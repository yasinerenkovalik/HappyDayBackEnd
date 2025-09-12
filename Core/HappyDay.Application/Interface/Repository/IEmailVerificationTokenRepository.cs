// Core/HappyDay.Application/Interface/Repository/IEmailVerificationTokenRepository.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository
{
    public interface IEmailVerificationTokenRepository : IGenericRepository<EmailVerificationToken>
    {
        Task<EmailVerificationToken?> GetValidAsync(Guid companyId, string tokenHash, string purpose, CancellationToken ct);
    }
}