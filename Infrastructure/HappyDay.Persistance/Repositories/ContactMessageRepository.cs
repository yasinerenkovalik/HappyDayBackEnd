using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories;

public class ContactMessageRepository:GenericRepository<ContactMessage>,IContactMessageRepository
{
    private readonly HappyDayContext _context;
    public ContactMessageRepository(HappyDayContext appContext, HappyDayContext context) : base(appContext)
    {
        _context = context;
    }

    public async Task<List<ContactMessage>> GetByCompany(Guid companyId)
    {
        return await _context.ContactMessages.Where(o => o.CompanyId == companyId && o.IsActivated==true).ToListAsync();
    }
}