using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class OrganizationRepository:GenericRepository<Organization>,IOrganizationRepository
{
    private readonly HappyDayContext _context;
    public OrganizationRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
    }

    public async Task<GetOrganizationWithImagesResponse> GetOrganizationWithImages(Guid Id)
    {
        var result = await _context.Organizations
            .Where(o => o.Id == Id)
            .Select(o => new GetOrganizationWithImagesResponse
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Price = o.Price,
                MaxGuestCount = o.MaxGuestCount,
                Images = o.OrganizationImages
                    .Where(img => img.IsActivated == true)
                    .Select(img => new OrganizationImageDto
                    {
                        Id = img.Id,
                        ImageUrl = img.ImageUrl
                    }).ToList(),
           
                Duration = o.Duration,
                Services = o.Services,
                IsOutdoor = o.IsOutdoor,
                ReservationNote = o.ReservationNote,
                CancelPolicy = o.CancelPolicy,
                VideoUrl = o.VideoUrl,
                CoverPhotoPath = o.CoverPhotoPath,
            })
            .FirstOrDefaultAsync();

        return result;
    }


    public async Task<List<Organization>> GetByCompany(Guid companyId)
    {
        return await _context.Organizations.Where(o => o.CompanyId == companyId).ToListAsync();
    }

    public async Task<List<Organization>> GetFilteredAsync(GetFilteredOrganizationsQueryRequest  request)
    {
        var query = _context.Organizations
            .Include(x => x.City)
            .Include(x => x.District)
            .AsQueryable();

        if (request.CityId.HasValue)
            query = query.Where(x => x.CityId == request.CityId);

        if (request.DistrictId.HasValue)
            query = query.Where(x => x.DistrictId == request.DistrictId);

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == request.CategoryId);

        if (request.IsOutdoor.HasValue)
            query = query.Where(x => x.IsOutdoor == request.IsOutdoor);

        if (request.MaxPrice.HasValue)
            query = query.Where(x => x.Price <= request.MaxPrice);

        return await query.ToListAsync();
    }
}

