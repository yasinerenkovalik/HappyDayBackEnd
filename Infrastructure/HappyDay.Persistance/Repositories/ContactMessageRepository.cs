using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class ContactMessageRepository:GenericRepository<ContactMessage>,IContactMessageRepository
{
    private readonly HappyDayContext _context;
    public ContactMessageRepository(HappyDayContext appContext, HappyDayContext context) : base(appContext)
    {
        _context = context;
    }

    public async Task<List<ContactMessage>> GetByCompany(Guid companyId)
    {
        return await _context.ContactMessages.Where(o => o.CompanyId == companyId && o.IsActivated==true).ToListAsync();
    }

    public async Task<List<ContactMessageWithVenue>> GetByCompanyWithVenue(Guid companyId)
    {
        // ContactMessage üzerinde navigation property olmadığı için
        // mesajları organizasyonlarla OrganizationId üzerinden birleştiriyoruz.
        var messages = await _context.ContactMessages
            .AsNoTracking()
            .Where(m => m.CompanyId == companyId && m.IsActivated)
            .OrderByDescending(m => m.CreateDate)
            .ToListAsync();

        if (messages.Count == 0) return new List<ContactMessageWithVenue>();

        var orgIds = messages.Select(m => m.OrganizationId).Distinct().ToList();

        var orgLookup = await _context.Organizations
            .AsNoTracking()
            .Where(o => orgIds.Contains(o.Id))
            .Select(o => new
            {
                o.Id,
                o.Title,
                CityName = o.Company != null && o.Company.City != null ? o.Company.City.CityName : null,
                DistrictName = o.Company != null && o.Company.District != null ? o.Company.District.DistrictName : null,
            })
            .ToListAsync();

        var byId = orgLookup.ToDictionary(x => x.Id);

        return messages.Select(m =>
        {
            var row = new ContactMessageWithVenue
            {
                Id = m.Id,
                FullName = m.FullName,
                Phone = m.Phone,
                Email = m.Email,
                Message = m.Message,
                OrganizationId = m.OrganizationId,
                CompanyId = m.CompanyId,
                CreateDate = m.CreateDate,
            };

            if (byId.TryGetValue(m.OrganizationId, out var org))
            {
                row.OrganizationTitle = org.Title;
                row.CityName = org.CityName;
                row.DistrictName = org.DistrictName;
            }

            return row;
        }).ToList();
    }
    public async Task<List<ContactMessageAdminListItem>> GetAllForAdmin(string? search, int skip, int take)
    {
        var query = _context.ContactMessages
            .AsNoTracking()
            .Where(m => m.IsActivated);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(m =>
                m.FullName.ToLower().Contains(s) ||
                m.Email.ToLower().Contains(s) ||
                m.Phone.Contains(s));
        }

        var messages = await query
            .OrderByDescending(m => m.CreateDate)
            .Skip(skip)
            .Take(take)
            .Select(m => new { m.Id, m.FullName, m.Phone, m.Email, m.Message, m.CreateDate, m.CompanyId, m.OrganizationId })
            .ToListAsync();

        if (messages.Count == 0)
        {
            return new List<ContactMessageAdminListItem>();
        }

        var companyIds = messages.Select(m => m.CompanyId).Distinct().ToList();
        var orgIds = messages.Select(m => m.OrganizationId).Distinct().ToList();

        var companies = await _context.Set<Company>()
            .AsNoTracking()
            .Where(c => companyIds.Contains(c.Id))
            .Select(c => new { c.Id, c.Name })
            .ToDictionaryAsync(x => x.Id, x => x.Name);

        var orgs = await _context.Organizations
            .AsNoTracking()
            .Where(o => orgIds.Contains(o.Id))
            .Select(o => new
            {
                o.Id,
                o.Title,
                CityName = o.Company != null && o.Company.City != null ? o.Company.City.CityName : string.Empty,
                DistrictName = o.Company != null && o.Company.District != null ? o.Company.District.DistrictName : string.Empty
            })
            .ToDictionaryAsync(x => x.Id, x => x);

        return messages.Select(m => new ContactMessageAdminListItem
        {
            Id = m.Id,
            FullName = m.FullName,
            Phone = m.Phone,
            Email = m.Email,
            Message = m.Message,
            CreateDate = m.CreateDate,
            CompanyId = m.CompanyId,
            CompanyName = companies.TryGetValue(m.CompanyId, out var cn) ? cn : "Bilinmiyor",
            OrganizationTitle = orgs.TryGetValue(m.OrganizationId, out var org) ? org.Title : "Bilinmiyor",
            CityName = orgs.TryGetValue(m.OrganizationId, out var org2) ? org2.CityName : string.Empty,
            DistrictName = orgs.TryGetValue(m.OrganizationId, out var org3) ? org3.DistrictName : string.Empty
        }).ToList();
    }
}
