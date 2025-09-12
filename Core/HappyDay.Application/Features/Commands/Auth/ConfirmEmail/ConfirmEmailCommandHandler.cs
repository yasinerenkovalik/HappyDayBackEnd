// Core/HappyDay.Application/Features/Commands/Auth/ConfirmEmail/ConfirmEmailCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Features.EmailVerification;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Auth.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, GeneralResponse<ConfirmEmailCommandResponse>>
    {
        private readonly IEmailVerificationService _emailVerification;

        public ConfirmEmailCommandHandler(IEmailVerificationService emailVerification)
        {
            _emailVerification = emailVerification;
        }

        public async Task<GeneralResponse<ConfirmEmailCommandResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var ok = await _emailVerification.VerifyAsync(request.CompanyId, request.Token, cancellationToken);

            return new GeneralResponse<ConfirmEmailCommandResponse>
            {
                isSuccess = ok,
                Message = ok ? "E-posta doğrulandı." : "Geçersiz veya süresi dolmuş bağlantı.",
                Data = new ConfirmEmailCommandResponse { Confirmed = ok, Message = ok ? "OK" : "INVALID" }
            };
        }
    }
}