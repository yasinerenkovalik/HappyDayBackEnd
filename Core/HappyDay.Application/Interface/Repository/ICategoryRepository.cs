using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAysnc();
}