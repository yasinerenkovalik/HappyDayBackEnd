using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.AutLogin;

public class AuthLoginQueryRequest:IRequest<GeneralResponse<AuthLoginQueryResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}