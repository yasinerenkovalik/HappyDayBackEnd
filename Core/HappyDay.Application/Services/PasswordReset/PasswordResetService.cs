using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Common.Email;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace HappyDay.Application.Features.PasswordReset
{
    public interface IPasswordResetService
    {
        Task GenerateAndSendAsync(Guid companyId, string email, CancellationToken ct);
        Task<bool> ResetAsync(Guid companyId, string plainToken, string newPassword, CancellationToken ct);
    }

    public class PasswordResetService : IPasswordResetService
    {
        private readonly IPasswordResetTokenRepository _tokens;
        private readonly ICompanyRepository _companies;
        private readonly IEmailSender _email;
        private readonly IConfiguration _cfg;
        private readonly Common.Security.BcryptPasswordHasher _hasher;

        public PasswordResetService(
            IPasswordResetTokenRepository tokens,
            ICompanyRepository companies,
            IEmailSender email,
            IConfiguration cfg)
        {
            _tokens = tokens;
            _companies = companies;
            _email = email;
            _cfg = cfg;

            var sec = _cfg.GetSection("Security");
            var work = sec.GetValue<int?>("PasswordWorkFactor") ?? 12;
            var pepper = sec.GetValue<string>("Pepper");
            _hasher = new Common.Security.BcryptPasswordHasher(work, pepper);
        }

        public async Task GenerateAndSendAsync(Guid companyId, string email, CancellationToken ct)
        {
            // 1) random token (URL-safe)
            var plainToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+","-").Replace("/","_").TrimEnd('=');

            // 2) hash
            var tokenHash = Sha256(plainToken);

            // 3) db'ye kaydet
            var entity = new PasswordResetToken
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30), // 30 dk geçerli
                Purpose = "password_reset"
            };
            await _tokens.AddAsync(entity, ct);
 

            // 4) link
            var baseUrl = _cfg["Frontend:BaseUrl"] ?? "http://localhost:3000";
            var link = $"{baseUrl}/auth/reset-password?cid={companyId}&token={plainToken}";

            // 5) mail
            var subject = "Şifre sıfırlama isteği";
            var html = $@"
                <p>Merhaba,</p>
                <p>Şifrenizi sıfırlamak için aşağıdaki butona tıklayın:</p>
                <p>
                  <a href=""{link}"" target=""_blank""
                     style=""display:inline-block;padding:10px 16px;border-radius:8px;text-decoration:none;border:1px solid #eee;"">
                     Şifremi Sıfırla
                  </a>
                </p>
                <p>Bağlantı 30 dakika boyunca geçerlidir.</p>";

            await _email.SendAsync(email, subject, html, $"Şifre sıfırlama bağlantısı: {link}", ct);
        }

        public async Task<bool> ResetAsync(Guid companyId, string plainToken, string newPassword, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(plainToken) || string.IsNullOrWhiteSpace(newPassword))
                return false;

            var tokenHash = Sha256(plainToken);
            var token = await _tokens.GetValidAsync(companyId, tokenHash, "password_reset", ct);
            if (token is null) return false;

            var company = await _companies.GetByIdAsync(companyId);
            if (company is null) return false;

            company.PasswordHash = _hasher.Hash(newPassword);
            await _companies.UpdateAsync(company);

            token.UsedAt = DateTime.UtcNow;
            await _tokens.UpdateAsync(token);
         

            return true;
        }

        private static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }
    }
}
