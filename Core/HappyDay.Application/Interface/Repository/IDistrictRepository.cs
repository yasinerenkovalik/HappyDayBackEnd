using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IDistrictRepository
{
    Task<List<District>> GetDistrictsByCityIdAsync(int cityId);
}