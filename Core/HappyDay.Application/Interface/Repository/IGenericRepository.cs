using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Application.Interface.Repository;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAysnc();
    Task<T> GetByIdAsync(Guid Id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
    Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<PagedResult<TResult>> GetPagedAsync<TResult>(
        int pageNumber,
        int pageSize,
        CancellationToken ct,
        Func<IQueryable<T>, IQueryable<TResult>> selector,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        where TResult : class;
}