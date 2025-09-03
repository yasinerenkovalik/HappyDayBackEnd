using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Auth.OrganizationLogin;

public class CompanyLoginQueryRequest:IRequest<GeneralResponse<CompanyLoginQueryResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}