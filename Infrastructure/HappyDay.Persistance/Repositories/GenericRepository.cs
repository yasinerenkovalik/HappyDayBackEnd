using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Messages;
using HappyDay.Domain.Entities.BaseEntites;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class GenericRepository<T>:IGenericRepository<T> where T:BaseEntity
{
    private readonly HappyDayContext _appContext;
    public GenericRepository(HappyDayContext appContext)
    {
        _appContext=appContext;
    }
    public async Task<T> AddAsync(T entity)
    {
        entity.CreateDate = DateTime.UtcNow;
        entity.IsActivated=true;
        
        await _appContext.Set<T>().AddAsync(entity);
        await _appContext.SaveChangesAsync();
     
        return entity;
    }

    public async Task<T> DeleteAsync(Guid id)
    {

        var entity = await _appContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            throw new Exception(MessageConstants.RegisterNotFound);
        }
        entity.IsActivated=false;
        entity.DeleteDate = DateTime.UtcNow;
            
        _appContext.Set<T>().Update(entity);
           
        await _appContext.SaveChangesAsync();

        return entity;
    }



    public async Task<List<T>> GetAllAysnc()
    {
        return await _appContext.Set<T>().Where(o=>o.IsActivated==true).ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid Id)
    {
        return await _appContext.Set<T>().FirstOrDefaultAsync(x => x.Id == Id );
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdateDate = DateTime.UtcNow;
            
        _appContext.Set<T>().Update(entity);
            
        await _appContext.SaveChangesAsync();
            
        return entity;
    }
}