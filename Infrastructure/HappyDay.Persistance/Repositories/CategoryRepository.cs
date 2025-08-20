using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class CategoryRepository:ICategoryRepository
{
    private readonly HappyDayContext _context;
    public CategoryRepository(HappyDayContext context)
    {
        _context = context;
    }
    public async Task<List<Category>> GetAllAysnc()
    {
        return await _context.Set<Category>().ToListAsync();
    }
}