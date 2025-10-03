// Core/HappyDay.Application/Features/EmailVerification/EmailVerificationService.cs
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Common.Email;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace HappyDay.Application.Features.EmailVerification
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IEmailSender _email;
        private readonly IEmailVerificationTokenRepository _tokens;
        private readonly ICompanyRepository _companies;
        private readonly IConfiguration _cfg;

        public EmailVerificationService(IEmailSender email,
                                        IEmailVerificationTokenRepository tokens,
                                        ICompanyRepository companies,
                                        IConfiguration cfg)
        {
            _email = email;
            _tokens = tokens;
            _companies = companies;
            _cfg = cfg;
        }

        public async Task GenerateAndSendAsync(Guid companyId, string email, CancellationToken ct)
        {
            // 1) random token
            var plainToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
                .Replace("+", "-").Replace("/", "_").TrimEnd('=');

            // 2) hash
            var tokenHash = Sha256(plainToken);

            // 3) db kaydı
            var entity = new EmailVerificationToken
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.AddHours(24),
                Purpose = "email_confirm"
            };
            await _tokens.AddAsync(entity);

            // 4) link
            var frontBase = _cfg["Frontend:BaseUrl"] ?? "http://mutlugunum.com.tr/auth/confirm-email";
            var link = $"{frontBase}/auth/confirm-email?cid={companyId}&token={plainToken}";

            // 5) mail
            var subject = "E-posta adresinizi doğrulayın";
            var html = $@"
                <p>Merhaba,</p>
                <p>Hesabınızı etkinleştirmek için aşağıdaki butona tıklayın:</p>
                <p>
                  <a href=""{link}"" target=""_blank""
                     style=""display:inline-block;padding:10px 16px;border-radius:8px;text-decoration:none;border:1px solid #eee;"">
                     E-postamı Doğrula
                  </a>
                </p>
                <p>Bağlantı 24 saat boyunca geçerlidir.</p>";

            await _email.SendAsync(email, subject, html, $"Doğrulama bağlantısı: {link}", ct);
        }

        public async Task<bool> VerifyAsync(Guid companyId, string plainToken, CancellationToken ct)
        {
            var tokenHash = Sha256(plainToken);
            var token = await _tokens.GetValidAsync(companyId, tokenHash, "email_confirm", ct);
            if (token is null) return false;

            token.UsedAt = DateTime.UtcNow;
            await _tokens.UpdateAsync(token);

            var company = await _companies.GetByIdAsync(companyId);
            if (company is null) return false;

            company.IsEmailConfirmed = true;
            company.EmailConfirmedAt = DateTime.UtcNow;
            await _companies.UpdateAsync(company);

            return true;
        }

        private static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}
