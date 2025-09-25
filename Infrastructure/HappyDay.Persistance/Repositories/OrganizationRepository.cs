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
                CityName = o.Company.City.CityName, // 👈 City tablosundan Name alıyoruz
                DistrictName = o.Company.District.DistrictName, 
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

    public async Task<PagedResult<GetFilteredOrganizationsQueryResponse>> GetFilteredAsync(
        GetFilteredOrganizationsQueryRequest request,
        CancellationToken ct)
    {
        var query = _context.Organizations
            .Include(x => x.Company.City)
            .Include(x => x.Company.District)
            .Where(x => x.IsActivated == true)
            .AsNoTracking()
            .AsQueryable();

        if (request.CityId.HasValue)
            query = query.Where(x => x.Company.CityId == request.CityId);

        if (request.DistrictId.HasValue)
            query = query.Where(x => x.Company.DistrictId == request.DistrictId);

        if (request.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == request.CategoryId);

        if (request.IsOutdoor.HasValue)
            query = query.Where(x => x.IsOutdoor == request.IsOutdoor);

        if (request.MaxPrice.HasValue)
            query = query.Where(x => x.Price <= request.MaxPrice);

        // 1) toplam kayıt
        var totalCount = await query.CountAsync(ct);

        // 2) sıralama (CreatedAt varsa onu kullan; yoksa Id/Title vb.)
        query = query.OrderByDescending(x => x.CreateDate);

        // 3) sayfalama
        var skip = (request.Page - 1) * request.PageSize;

        var items = await query
            .Skip(skip)
            .Take(request.PageSize)
            .Select(x => new GetFilteredOrganizationsQueryResponse
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                CityId = x.Company.CityId,
                CityName = x.Company.City.CityName,
                DistrictId = x.Company.DistrictId,
                DistrictName = x.Company.District.DistrictName,
                CoverPhotoPath = x.CoverPhotoPath,
            })
            .ToListAsync(ct);

        return new PagedResult<GetFilteredOrganizationsQueryResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
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

