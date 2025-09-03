using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IOrganizationImagesRepository:IGenericRepository<OrganizationImage>
{
    Task<OrganizationImage> DeleteOrganizationImageAsync(int id);
}