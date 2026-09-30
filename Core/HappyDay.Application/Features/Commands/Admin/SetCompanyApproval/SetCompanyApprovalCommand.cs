using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Admin.SetCompanyApproval;

public class SetCompanyApprovalCommand : IRequest<GeneralResponse<SetCompanyApprovalResponse>>
{
    public Guid CompanyId { get; set; }
    public bool IsApproved { get; set; }
}

public class SetCompanyApprovalResponse
{
    public Guid CompanyId { get; set; }
    public bool IsApproved { get; set; }
}
