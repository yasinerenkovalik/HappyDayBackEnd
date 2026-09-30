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
        
       var result=await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActivated);
       if (result == null)
       {
           return null;
       }
     
       return result;
       
    }

    public async Task<List<UserAdminListItem>> GetAllForAdmin(string? search, int skip, int take)
    {
        var query = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(u =>
                u.Name.ToLower().Contains(s) ||
                u.SurName.ToLower().Contains(s) ||
                u.Email.ToLower().Contains(s));
        }

        return await query
            .OrderByDescending(u => u.CreateDate)
            .Skip(skip)
            .Take(take)
            .Select(u => new UserAdminListItem
            {
                Id = u.Id,
                Name = u.Name,
                SurName = u.SurName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                IsActivated = u.IsActivated,
                CreateDate = u.CreateDate,
                CompanyId = u.CompanyId,
                CompanyName = u.Company != null ? u.Company.Name : null
            })
            .ToListAsync();
    }

    public async Task<User?> GetByIdIncludingInactiveAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }
}
