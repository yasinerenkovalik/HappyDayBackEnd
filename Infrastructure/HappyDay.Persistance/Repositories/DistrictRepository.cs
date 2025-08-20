using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class DistrictRepository:IDistrictRepository
{
    private readonly HappyDayContext _context;

    public DistrictRepository(HappyDayContext context)
    {
        _context = context;
    }

    public async Task<List<District>> GetDistrictsByCityIdAsync(int cityId)
    {
        return await _context.Set<District>().Where(d => d.CityId == cityId).ToListAsync();
    }
}