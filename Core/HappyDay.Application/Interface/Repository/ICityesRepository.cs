using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICityesRepository
{
    Task<List<City>> GetAllAysnc();
}