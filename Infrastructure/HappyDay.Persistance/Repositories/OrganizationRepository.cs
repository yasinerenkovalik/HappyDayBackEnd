using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetFeatured;
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
            .Where(o => o.Id == Id && o.IsActivated == true)
            .Select(o => new GetOrganizationWithImagesResponse
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Price = o.Price,
                MaxGuestCount = o.MaxGuestCount,
                CategoryId = o.CategoryId,
                CityName = o.City.CityName, // 👈 City tablosundan Name alıyoruz
                DistrictName = o.District.DistrictName, 
                Latitude = o.Company.Latitude,
                Longitude = o.Company.Longitude,
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

    public async Task<List<GetFilteredOrganizationsQueryResponse>> GetFilteredAsync(GetFilteredOrganizationsQueryRequest request)
    {
        var query = _context.Organizations
            .Include(x => x.City)
            .Include(x => x.District)
            .Where(x => x.IsActivated == true)
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

        return await query
            .Select(x => new GetFilteredOrganizationsQueryResponse
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                CityId = x.CityId,
                CityName = x.City.CityName,
                DistrictId = x.DistrictId,
                DistrictName = x.District.DistrictName
            })
            .ToListAsync();
    }


    public async Task<List<Organization>> GetByCompany(Guid companyId)
    {
        return await _context.Organizations.Where(o => o.CompanyId == companyId && o.IsActivated==true).ToListAsync();
    }

   
    public async Task<List<Organization>> GetFeaturedAsync(GetFeaturedQueryRequest  request)
    {
        return await _context.Organizations
            .Where(o => o.IsFeatured && o.IsActivated && o.CategoryId==request.Id)
            .ToListAsync();
    }
}

