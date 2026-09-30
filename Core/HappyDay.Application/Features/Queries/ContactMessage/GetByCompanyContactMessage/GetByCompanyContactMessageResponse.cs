namespace HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;

public class GetByCompanyContactMessageResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreateDate { get; set; }

    /// <summary>Mesajın gönderildiği mekanın adı (yönetici listesi için).</summary>
    public string OrganizationTitle { get; set; }

    /// <summary>Mekanın şehri / ilçesi.</summary>
    public string CityName { get; set; }
    public string DistrictName { get; set; }
}
