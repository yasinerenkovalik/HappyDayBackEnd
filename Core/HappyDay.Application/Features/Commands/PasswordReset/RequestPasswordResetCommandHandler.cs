using MediatR;
using HappyDay.Application.Wrappers;
using HappyDay.Application.Interface.Repository;

namespace HappyDay.Application.Features.PasswordReset.RequestPasswordReset;

public class RequestPasswordResetCommandHandler
    : IRequestHandler<RequestPasswordResetCommand, GeneralResponse<Unit>>
{
    private readonly ICompanyRepository _companies;
    private readonly PasswordReset.IPasswordResetService _service;

    public RequestPasswordResetCommandHandler(
        ICompanyRepository companies,
        PasswordReset.IPasswordResetService service)
    {
        _companies = companies;
        _service = service;
    }

    public async Task<GeneralResponse<Unit>> Handle(RequestPasswordResetCommand req, CancellationToken ct)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        var company = await _companies.GetByEmailAsync(email);

        // Güvenlik: kullanıcıya "bu mail var mı yok mu" demiyoruz
        if (company is not null)
        {
            await _service.GenerateAndSendAsync(company.Id, company.Email, ct);
        }

        return new GeneralResponse<Unit>
        {
            isSuccess = true,
            Message = "Eğer e-posta kayıtlıysa şifre sıfırlama bağlantısı gönderildi."
        };
    }
}