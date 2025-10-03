using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Models;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class CalanderRepository:GenericRepository<CalendarEvent>, ICalanderEvenetRepository
{
    public CalanderRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}