using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

/// <summary>Bir mesajın gönderildiği mekan bilgisiyle birlikte taşınan görünümü.</summary>
public class ContactMessageWithVenue
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid CompanyId { get; set; }
    public DateTime CreateDate { get; set; }

    public string OrganizationTitle { get; set; }
    public string CityName { get; set; }
    public string DistrictName { get; set; }
}

public interface IContactMessageRepository:IGenericRepository<ContactMessage>
{
    Task<List<ContactMessage>> GetByCompany(Guid companyId);

    /// <summary>Şirkete gelen mesajları, bağlı oldukları mekanın adı ve şehri ile birlikte döner.</summary>
    Task<List<ContactMessageWithVenue>> GetByCompanyWithVenue(Guid companyId);

    /// <summary>Admin paneli: tüm mesajlar, firma ve mekan bilgisiyle birlikte.</summary>
    Task<List<ContactMessageAdminListItem>> GetAllForAdmin(string? search, int skip, int take);
}

/// <summary>Admin paneli mesaj liste satırı.</summary>
public class ContactMessageAdminListItem
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime CreateDate { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = default!;
    public string OrganizationTitle { get; set; } = default!;
    public string CityName { get; set; } = default!;
    public string DistrictName { get; set; } = default!;
}
