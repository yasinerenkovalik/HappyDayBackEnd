using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Persistance.Repositories;

public class ContactMessageRepository:GenericRepository<ContactMessage>,IContactMessageRepository
{
    public ContactMessageRepository(HappyDayContext appContext) : base(appContext)
    {
    }
}