// Core/HappyDay.Application/Features/EmailVerification/IEmailVerificationService.cs
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HappyDay.Application.Features.EmailVerification
{
    public interface IEmailVerificationService
    {
        Task GenerateAndSendAsync(Guid companyId, string email, CancellationToken ct);
        Task<bool> VerifyAsync(Guid companyId, string plainToken, CancellationToken ct);
    }
}