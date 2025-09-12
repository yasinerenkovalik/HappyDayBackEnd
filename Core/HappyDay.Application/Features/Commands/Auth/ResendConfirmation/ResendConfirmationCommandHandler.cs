// Core/HappyDay.Application/Features/Commands/Auth/ResendConfirmation/ResendConfirmationCommandHandler.cs
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Features.EmailVerification;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Auth.ResendConfirmation
{
    public class ResendConfirmationCommandHandler : IRequestHandler<ResendConfirmationCommand, GeneralResponse<ResendConfirmationCommandResponse>>
    {
        private readonly ICompanyRepository _companies;
        private readonly IEmailVerificationService _emailVerification;

        public ResendConfirmationCommandHandler(ICompanyRepository companies, IEmailVerificationService emailVerification)
        {
            _companies = companies;
            _emailVerification = emailVerification;
        }

        public async Task<GeneralResponse<ResendConfirmationCommandResponse>> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var company = await _companies.GetByEmailAsync(request.Email);
            if (company is null)
            {
                // Bilgi sızdırmamak için success dön
                return new GeneralResponse<ResendConfirmationCommandResponse>
                {
                    isSuccess = true,
                    Message = "Eğer böyle bir hesap varsa mail gönderildi.",
                    Data = new ResendConfirmationCommandResponse { Sent = true, Message = "OK" }
                };
            }

            if (company.IsEmailConfirmed)
            {
                return new GeneralResponse<ResendConfirmationCommandResponse>
                {
                    isSuccess = true,
                    Message = "E-posta zaten doğrulanmış.",
                    Data = new ResendConfirmationCommandResponse { Sent = false, Message = "ALREADY_CONFIRMED" }
                };
            }

            await _emailVerification.GenerateAndSendAsync(company.Id, company.Email, cancellationToken);

            return new GeneralResponse<ResendConfirmationCommandResponse>
            {
                isSuccess = true,
                Message = "Onay e-postası gönderildi.",
                Data = new ResendConfirmationCommandResponse { Sent = true, Message = "OK" }
            };
        }
    }
}
