using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class ContactMessage : BaseEntity
{
    public string FullName { get; set; } 
    public string Phone { get; set; }
    public string Email { get; set; } 
    public string Message { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CompanyId { get; set; }
}