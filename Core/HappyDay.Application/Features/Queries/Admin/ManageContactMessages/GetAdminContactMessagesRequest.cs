using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Admin.ManageContactMessages;

public class GetAdminContactMessagesRequest : IRequest<GeneralResponse<List<GetAdminContactMessagesItem>>>
{
    public string? Search { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetAdminContactMessagesItem
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
