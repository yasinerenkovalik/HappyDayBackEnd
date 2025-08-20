using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class ReservationRepository:GenericRepository<Reservation>,IReservasyonRepository
{
    public ReservationRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}