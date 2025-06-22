using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IOrganizationRepository:IGenericRepository<Organization>
{
    Task<GetOrganizationWithImagesResponse> GetOrganizationWithImages(Guid Id);
    Task<List<Organization>> GetByCompany(Guid companyId);
}