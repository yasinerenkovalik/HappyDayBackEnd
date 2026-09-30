using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageCompanies;

public class GetAdminCompaniesRequest : IRequest<GeneralResponse<List<GetAdminCompaniesItem>>>
{
    /// <summary>Arama metni (firma adı / e-posta).</summary>
    public string? Search { get; set; }

    /// <summary>Onay durumu filtresi.</summary>
    public bool? IsApproved { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetAdminCompaniesItem
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
