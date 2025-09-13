using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly HappyDayContext _context;
        public PasswordResetTokenRepository(HappyDayContext context) => _context = context;

        // KAYIT EKLE → SaveChangesAsync burada
        public async Task AddAsync(PasswordResetToken token, CancellationToken ct = default)
        {
            await _context.PasswordResetTokens.AddAsync(token, ct);
            await _context.SaveChangesAsync(ct);
        }

        // GEÇERLİ TOKEN GETİR
        public Task<PasswordResetToken?> GetValidAsync(Guid companyId, string tokenHash, string purpose, CancellationToken ct = default)
        {
            return _context.PasswordResetTokens.FirstOrDefaultAsync(t =>
                t.CompanyId == companyId &&
                t.TokenHash == tokenHash &&
                t.Purpose   == purpose &&
                t.UsedAt    == null &&
                t.ExpiresAt > DateTime.UtcNow, ct);
        }

        // GÜNCELLE → SaveChangesAsync burada
        public async Task UpdateAsync(PasswordResetToken token, CancellationToken ct = default)
        {
            _context.PasswordResetTokens.Update(token);
            await _context.SaveChangesAsync(ct);
        }

        // (Teşhis için) SAY
        public Task<int> CountByCompanyAsync(Guid companyId, CancellationToken ct = default)
        {
            return _context.PasswordResetTokens.CountAsync(t => t.CompanyId == companyId, ct);
        }
    }
}