using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICityesRepository
{
    Task<List<City>> GetAllAysnc();

    /// <summary>Konum tespiti için koordinatı olan tüm iller.</summary>
    Task<List<City>> GetAllWithCoordinatesAsync();
}
