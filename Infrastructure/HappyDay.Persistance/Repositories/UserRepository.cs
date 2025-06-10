using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class UserRepository:GenericRepository<User>, IUserRepository
{
    public UserRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}