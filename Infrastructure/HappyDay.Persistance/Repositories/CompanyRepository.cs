using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class CompanyRepository:GenericRepository<Company>,ICompanyRepository
{
    private readonly HappyDayContext _context;
    public CompanyRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
    }

    public async Task<Company?> GetByEmailAsync(string email)
    {
        var result=await _context.Companies.FirstOrDefaultAsync(u => u.Email == email && u.IsActivated);
        if (result == null)
        {
            return null;
        }
     
        return result;
    }

    public async Task<List<CompanyAdminListItem>> GetAllForAdmin(string? search, bool? isApproved, int skip, int take)
    {
        var query = _context.Companies.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(s) || c.Email.ToLower().Contains(s));
        }

        if (isApproved.HasValue)
        {
            query = query.Where(c => c.IsApproved == isApproved.Value);
        }

        var items = await query
            .OrderByDescending(c => c.CreateDate)
            .Skip(skip)
            .Take(take)
            .Select(c => new CompanyAdminListItem
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Adress = c.Adress,
                IsApproved = c.IsApproved,
                IsEmailConfirmed = c.IsEmailConfirmed,
                IsActivated = c.IsActivated,
                CreateDate = c.CreateDate,
                OrganizationCount = c.Organizations.Count(o => o.IsActivated),
                UserCount = _context.Users.Count(u => u.CompanyId == c.Id && u.IsActivated),
                MessageCount = _context.ContactMessages.Count(m => m.CompanyId == c.Id && m.IsActivated)
            })
            .ToListAsync();

        return items;
    }

    public async Task<(int Total, int Approved, int Pending)> GetApprovalCountsAsync()
    {
        var total = await _context.Companies.CountAsync(c => c.IsActivated);
        var approved = await _context.Companies.CountAsync(c => c.IsActivated && c.IsApproved);
        var pending = await _context.Companies.CountAsync(c => c.IsActivated && !c.IsApproved);
        return (total, approved, pending);
    }
}
