using MediatR;
using HappyDay.Application.Wrappers;

namespace HappyDay.Application.Features.PasswordReset.ResetPassword;

public class ResetPasswordCommand : IRequest<GeneralResponse<Unit>>
{
    public Guid CompanyId { get; set; }
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}