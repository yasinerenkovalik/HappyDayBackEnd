using MediatR;
using HappyDay.Application.Wrappers;

namespace HappyDay.Application.Features.PasswordReset.ResetPassword;

public class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, GeneralResponse<Unit>>
{
    private readonly PasswordReset.IPasswordResetService _service;

    public ResetPasswordCommandHandler(PasswordReset.IPasswordResetService service)
    {
        _service = service;
    }

    public async Task<GeneralResponse<Unit>> Handle(ResetPasswordCommand req, CancellationToken ct)
    {
        var ok = await _service.ResetAsync(req.CompanyId, req.Token, req.NewPassword, ct);

        return ok
            ? new GeneralResponse<Unit> { isSuccess = true, Message = "Şifreniz başarıyla güncellendi." }
            : new GeneralResponse<Unit> { isSuccess = false, Message = "Geçersiz veya süresi dolmuş bağlantı." };
    }
}