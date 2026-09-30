using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageUsers;

public class GetAdminUsersRequest : IRequest<GeneralResponse<List<GetAdminUsersItem>>>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetAdminUsersItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string SurName { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public bool IsActivated { get; set; }
    public DateTime CreateDate { get; set; }

    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
}
