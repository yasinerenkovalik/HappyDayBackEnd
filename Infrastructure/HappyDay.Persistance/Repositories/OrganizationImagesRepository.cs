using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class OrganizationImagesRepository:GenericRepository<OrganizationImage>,IOrganizationImagesRepository
{
    public OrganizationImagesRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}