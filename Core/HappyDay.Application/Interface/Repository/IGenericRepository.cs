using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Application.Interface.Repository;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAysnc();
    Task<T> GetByIdAsync(Guid Id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(Guid id);
}