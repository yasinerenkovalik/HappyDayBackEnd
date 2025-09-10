using System.Linq.Expressions;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Messages;
using HappyDay.Domain.Entities.BaseEntites;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
// AutoMapper'lı alternatif için:
// using AutoMapper;
// using AutoMapper.QueryableExtensions;

namespace HappyDay.Persistance.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly HappyDayContext _appContext;
    public GenericRepository(HappyDayContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<T> AddAsync(T entity)
    {
        entity.CreateDate = DateTime.UtcNow;
        entity.IsActivated = true;

        await _appContext.Set<T>().AddAsync(entity);
        await _appContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> DeleteAsync(Guid id)
    {
        var entity = await _appContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            throw new Exception(MessageConstants.RegisterNotFound);

        entity.IsActivated = false;
        entity.DeleteDate = DateTime.UtcNow;

        _appContext.Set<T>().Update(entity);
        await _appContext.SaveChangesAsync();

        return entity;
    }

    // --- Mevcut basit sayfalama (entity döner) ---
    public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _appContext.Set<T>()
            .Where(x => x.IsActivated == true);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    // --- PROJEKSIYON DESTEKLI GENEL SAYFALAMA (CityName/DistrictName gibi alanlar için ÖNERİLEN) ---
    public async Task<PagedResult<TResult>> GetPagedAsync<TResult>(
        int pageNumber,
        int pageSize,
        CancellationToken ct,
        Func<IQueryable<T>, IQueryable<TResult>> selector,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        where TResult : class
    {
        IQueryable<T> baseQuery = _appContext.Set<T>()
            .Where(x => x.IsActivated)
            .AsNoTracking();

        var totalCount = await baseQuery.CountAsync(ct);

        if (orderBy != null)
            baseQuery = orderBy(baseQuery);

        var pageItems = await selector(baseQuery)      // 👈 DTO projeksiyonu burada
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<TResult>
        {
            Items = pageItems,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    // --- (Opsiyonel) AutoMapper ProjectTo kullanan alternatif ---
    // public async Task<PagedResult<TResult>> GetPagedProjectedAsync<TResult>(
    //     int pageNumber,
    //     int pageSize,
    //     CancellationToken ct,
    //     Expression<Func<T, bool>>? filter = null,
    //     Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
    //     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    //     IConfigurationProvider? mapperConfig = null,
    //     bool asNoTracking = true)
    //     where TResult : class
    // {
    //     if (mapperConfig is null)
    //         throw new ArgumentException("mapperConfig is required for ProjectTo");

    //     IQueryable<T> query = _appContext.Set<T>().Where(x => x.IsActivated == true);

    //     if (filter is not null)
    //         query = query.Where(filter);

    //     if (include is not null)
    //         query = include(query);

    //     if (asNoTracking)
    //         query = query.AsNoTracking();

    //     var totalCount = await query.CountAsync(ct);

    //     if (orderBy is not null)
    //         query = orderBy(query);

    //     var projected = query
    //         .ProjectTo<TResult>(mapperConfig)
    //         .Skip((pageNumber - 1) * pageSize)
    //         .Take(pageSize);

    //     var items = await projected.ToListAsync(ct);

    //     return new PagedResult<TResult>
    //     {
    //         Items = items,
    //         TotalCount = totalCount,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize
    //     };
    // }

    public async Task<List<T>> GetAllAysnc()
    {
        return await _appContext.Set<T>()
            .Where(o => o.IsActivated == true)
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid Id)
    {
        return await _appContext.Set<T>()
            .FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdateDate = DateTime.UtcNow;

        _appContext.Set<T>().Update(entity);
        await _appContext.SaveChangesAsync();

        return entity;
    }
}
