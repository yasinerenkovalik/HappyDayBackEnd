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
            .Where(o => o.Id == Id
                    && o.IsActivated
                    && o.Company.IsActivated
                    && o.Company.IsApproved)
            .Select(o => new GetOrganizationWithImagesResponse
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Price = o.Price,
                MaxGuestCount = o.MaxGuestCount,
                CategoryId = o.CategoryId,
                CompanyId = o.CompanyId,
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
    // Herkese acik listeye yalnizca YAYINDA olan mekanlar girer:
    // mekan aktif + sirket aktif + sirket onayli.
    var query = _context.Organizations
        .Include(x => x.Company.City)
        .Include(x => x.Company.District)
        .Where(x => x.IsActivated
                 && x.Company.IsActivated
                 && x.Company.IsApproved)
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

    if (request.MinCapacity.HasValue)
        query = query.Where(x => x.MaxGuestCount >= request.MinCapacity);

    if (request.MaxCapacity.HasValue)
        query = query.Where(x => x.MaxGuestCount <= request.MaxCapacity);

    if (!string.IsNullOrWhiteSpace(request.Service))
    {
        var service = request.Service.Trim().ToLower();
        query = query.Where(x => x.Services.Any(s => s.ToLower().Contains(service)));
    }

    // 1) toplam kayıt
    var totalCount = await query.CountAsync(ct);

    // 2) sıralama (fiyata göre veya default olarak tarihe göre)
    if (request.SortByPriceAsc.HasValue)
    {
        if (request.SortByPriceAsc.Value)
            query = query.OrderBy(x => x.Price);
        else
            query = query.OrderByDescending(x => x.Price);
    }
    else
    {
        query = query.OrderByDescending(x => x.CreateDate);
    }

    // 3) sayfalama
    var skip = (request.Page - 1) * request.PageSize;

    var items = await query
        .Skip(skip)
        .Take(request.PageSize)
        .Select(x => new GetFilteredOrganizationsQueryResponse
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            Price = x.Price,
            MaxGuestCount = x.MaxGuestCount,
            Services = x.Services,
            Duration = x.Duration,
            IsOutdoor = x.IsOutdoor,
            ReservationNote = x.ReservationNote,
            CancelPolicy = x.CancelPolicy,
            VideoUrl = x.VideoUrl,
            CompanyId = x.CompanyId,
            CityId = x.Company.CityId,
            CityName = x.Company.City.CityName,
            DistrictId = x.Company.DistrictId,
            DistrictName = x.Company.District.DistrictName,
            CoverPhotoPath = x.CoverPhotoPath,
            Longitude = x.Company.Longitude,
            Latitude = x.Company.Latitude,
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



    public Task<Organization?> GetPublishedByIdAsync(Guid id)
    {
        return _context.Organizations
            .Include(o => o.Company)
            .Include(o => o.Packages)
            .Include(o => o.OrganizationImages)
            .FirstOrDefaultAsync(o => o.Id == id
                                   && o.IsActivated
                                   && o.Company.IsActivated
                                   && o.Company.IsApproved);
    }

    public Task<int> CountPublishedByCityAsync(int cityId)
    {
        return _context.Organizations.CountAsync(o =>
            o.IsActivated
            && o.Company.IsActivated
            && o.Company.IsApproved
            && o.Company.CityId == cityId);
    }

    public Task<int> CountPublishedAsync()
    {
        return _context.Organizations.CountAsync(o =>
            o.IsActivated && o.Company.IsActivated && o.Company.IsApproved);
    }

    public async Task<List<Organization>> GetByCompany(Guid companyId)
    {
        // Firma paneli kendi kayitlarini onay durumundan bagimsiz gormelidir.
        return await _context.Organizations
            .Where(o => o.CompanyId == companyId && o.IsActivated)
            .ToListAsync();
    }

   
    public async Task<List<Organization>> GetFeaturedAsync(GetFeaturedQueryRequest  request)
    {
        return await _context.Organizations
            .Where(o => o.IsFeatured
                    && o.IsActivated
                    && o.Company.IsActivated
                    && o.Company.IsApproved
                    && o.CategoryId == request.Id)
            .ToListAsync();
    }
}

