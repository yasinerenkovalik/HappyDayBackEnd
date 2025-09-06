using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class ContactRepository:GenericRepository<Contact>,IContactRepository
{
    public ContactRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}