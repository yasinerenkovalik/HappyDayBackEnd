using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IContactMessageRepository:IGenericRepository<ContactMessage>
{
    Task<List<ContactMessage>> GetByCompany(Guid companyId);
}