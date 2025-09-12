// Infrastructure/HappyDay.Persistance/Repositories/EmailVerificationTokenRepository.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories
{
    public class EmailVerificationTokenRepository 
        : GenericRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
    {
        private readonly HappyDayContext _context;

        public EmailVerificationTokenRepository(HappyDayContext appContext, HappyDayContext context) : base(appContext)
        {
            _context = context;
        }

        public async Task<EmailVerificationToken?> GetValidAsync(Guid companyId, string tokenHash, string purpose, CancellationToken ct)
        {
            return await _context.EmailVerificationTokens
                .FirstOrDefaultAsync(t =>
                        t.CompanyId == companyId &&
                        t.TokenHash == tokenHash &&
                        t.Purpose == purpose &&
                        t.UsedAt == null &&
                        t.ExpiresAt > DateTime.UtcNow,
                    ct);
        }
    }
}