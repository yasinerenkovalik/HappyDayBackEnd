using MediatR;
using HappyDay.Application.Wrappers;

namespace HappyDay.Application.Features.PasswordReset.RequestPasswordReset;

public class RequestPasswordResetCommand : IRequest<GeneralResponse<Unit>>
{
    public string Email { get; set; } = string.Empty;
}