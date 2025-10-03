namespace HappyDay.Application.Interface.Repository;

public interface IPackageRepository:IGenericRepository<Package>
{
    Task<List<Package>> GetByOrganization(Guid companyId);
}