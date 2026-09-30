using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetFeatured;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IOrganizationRepository:IGenericRepository<Organization>
{
    Task<List<Organization>> GetFeaturedAsync(GetFeaturedQueryRequest request);
    Task<GetOrganizationWithImagesResponse> GetOrganizationWithImages(Guid Id);
    Task<List<Organization>> GetByCompany(Guid companyId);
    
    Task<PagedResult<GetFilteredOrganizationsQueryResponse>> GetFilteredAsync(
        GetFilteredOrganizationsQueryRequest request,
        CancellationToken ct);

    /// <summary>Yayinda olan mekani getirir (aktif + sirketi aktif ve onayli).</summary>
    Task<Organization?> GetPublishedByIdAsync(Guid id);

    /// <summary>Herkese acik listelerde gorunmesi gereken (aktif + sirketi onayli) mekanlar.</summary>
    Task<int> CountPublishedByCityAsync(int cityId);

    Task<int> CountPublishedAsync();
   

    
}