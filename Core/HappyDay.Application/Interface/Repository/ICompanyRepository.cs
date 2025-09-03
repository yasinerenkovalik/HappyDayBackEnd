using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICompanyRepository: IGenericRepository<Company>
{
    Task<Company?> GetByEmailAsync(string email);
}