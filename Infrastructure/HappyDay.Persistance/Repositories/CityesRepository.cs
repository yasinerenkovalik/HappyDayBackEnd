using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class CityesRepository:ICityesRepository
{
    private readonly HappyDayContext _context;
    public CityesRepository(HappyDayContext context)
    {
        _context = context;
    }
    public async Task<List<City>> GetAllAysnc()
    {
        return await _context.Set<City>().OrderBy(c => c.Id).ToListAsync();
    }
}