namespace HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;

public class GetByCompanyContactMessageResponse
{
    public string FullName { get; set; } 
    public string Phone { get; set; }
    public string Email { get; set; } 
    public string Message { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CompanyId { get; set; }
}