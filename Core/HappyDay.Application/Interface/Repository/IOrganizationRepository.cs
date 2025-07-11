using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IOrganizationRepository:IGenericRepository<Organization>
{
    Task<List<Organization>> GetFeaturedAsync();
    Task<GetOrganizationWithImagesResponse> GetOrganizationWithImages(Guid Id);
    Task<List<Organization>> GetByCompany(Guid companyId);
    Task<List<Organization>> GetFilteredAsync(
        GetFilteredOrganizationsQueryRequest request);

    
}