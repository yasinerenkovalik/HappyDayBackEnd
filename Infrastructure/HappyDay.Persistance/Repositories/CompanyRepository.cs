using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class CompanyRepository:GenericRepository<Company>,ICompanyRepository
{
    private readonly HappyDayContext _context;
    public CompanyRepository(HappyDayContext appContext) : base(appContext)
    {
        _context = appContext;
    }

    public async Task<Company?> GetByEmailAsync(string email)
    {
        var result=await _context.Companies.FirstOrDefaultAsync(u => u.Email == email);
        if (result == null)
        {
            return null;
        }
     
        return result;
    }
}