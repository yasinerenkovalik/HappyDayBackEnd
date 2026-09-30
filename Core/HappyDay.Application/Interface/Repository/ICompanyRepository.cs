using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICompanyRepository: IGenericRepository<Company>
{
    Task<Company?> GetByEmailAsync(string email);

    /// <summary>Admin paneli: tüm firmalar, mekan/kullanıcı/mesaj sayılarıyla birlikte.</summary>
    Task<List<CompanyAdminListItem>> GetAllForAdmin(string? search, bool? isApproved, int skip, int take);

    /// <summary>Admin paneli dashboard sayaçları.</summary>
    Task<(int Total, int Approved, int Pending)> GetApprovalCountsAsync();
}

/// <summary>Admin paneli firma liste satırı.</summary>
public class CompanyAdminListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string Adress { get; set; } = default!;
    public bool IsApproved { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public bool IsActivated { get; set; }
    public DateTime CreateDate { get; set; }
    public int OrganizationCount { get; set; }
    public int UserCount { get; set; }
    public int MessageCount { get; set; }
}