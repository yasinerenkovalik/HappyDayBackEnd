using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class CompanyRepository:GenericRepository<Company>,ICompanyRepository
{
    public CompanyRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}