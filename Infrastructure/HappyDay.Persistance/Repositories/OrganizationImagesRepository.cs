using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Messages;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class OrganizationImagesRepository:GenericRepository<OrganizationImage>,IOrganizationImagesRepository
{
    private readonly HappyDayContext _context;
    public OrganizationImagesRepository(HappyDayContext appContext, HappyDayContext context) : base(appContext)
    {
        _context = context;
    }
    public async Task<OrganizationImage> DeleteOrganizationImageAsync(int id)
    {

        var entity = await _context.Set<OrganizationImage>().FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            throw new Exception(MessageConstants.RegisterNotFound);
        }
        entity.IsActivated=false;
        entity.DeleteDate = DateTime.UtcNow;
            
        _context.Set<OrganizationImage>().Update(entity);
           
        await _context.SaveChangesAsync();

        return entity;
    }
 
}