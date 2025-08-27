using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IUserRepository:IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}