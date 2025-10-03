using HappyDay.Application.Interface.Repository;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class PackageRepository:GenericRepository<Package>,IPackageRepository
{
    private readonly HappyDayContext _context;
    public PackageRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
    }

    public async Task<List<Package>> GetByOrganization(Guid organizationId)
    {
        return await _context.Packages.Where(p => p.OrganizationId == organizationId && p.IsActivated==true).ToListAsync();
    }
}