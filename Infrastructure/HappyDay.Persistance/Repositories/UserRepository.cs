using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using HappyDay.Persistance.Security;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class UserRepository:GenericRepository<User>, IUserRepository
{
    private readonly HappyDayContext _context;

    public UserRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
       
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        
       var result=await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
       if (result == null)
       {
           return null;
       }
     
       return result;
       
    }
}