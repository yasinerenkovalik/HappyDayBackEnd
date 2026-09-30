using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using HappyDay.Application.Wrappers;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Features.EmailVerification;
using HappyDay.Application.Common.Security;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Features.Invitations.RegisterByInvite;

public class RegisterByInviteCommandHandler
    : IRequestHandler<RegisterByInviteCommand, GeneralResponse<RegisterByInviteResponse>>
{
    private readonly ICompanyRepository _companies;
    private readonly ICompanyInvitationService _invitations;
    private readonly IEmailVerificationService _emailVerification;
    private readonly BcryptPasswordHasher _hasher;
    private readonly ILogger<RegisterByInviteCommandHandler> _logger;

    public RegisterByInviteCommandHandler(
        ICompanyRepository companies,
        ICompanyInvitationService invitations,
        IEmailVerificationService emailVerification,
        IConfiguration config,
        ILogger<RegisterByInviteCommandHandler> logger)
    {
        _companies = companies;
        _invitations = invitations;
        _emailVerification = emailVerification;
        _logger = logger;

        var sec    = config.GetSection("Security");
        var work   = sec.GetValue<int?>("PasswordWorkFactor") ?? 12;
        var pepper = sec.GetValue<string>("Pepper");
        _hasher = new BcryptPasswordHasher(work, pepper);
    }

    public async Task<GeneralResponse<RegisterByInviteResponse>> Handle(
        RegisterByInviteCommand req, CancellationToken ct)
    {
        // 0) Basit doğrulamalar
        if (string.IsNullOrWhiteSpace(req.Token) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.Password) ||
            string.IsNullOrWhiteSpace(req.CompanyName))
        {
            return Fail("Zorunlu alanlar eksik.");
        }

        // 1) Davet doğrula
        var v = await _invitations.ValidateAsync(req.Token, ct);
        if (!v.IsValid) return Fail(v.Reason ?? "Token geçersiz.");

        // 2) E-mail benzersizlik
        var email = req.Email.Trim().ToLowerInvariant();
        var exists = await _companies.GetByEmailAsync(email);
        if (exists is not null) return Fail("Bu e-posta ile kayıt mevcut.");

        // 3) Hashle
        var pwdHash = _hasher.Hash(req.Password);
        _logger.LogInformation("Kullanici olusturuldu: {Email}", req.Email);

        // 4) Company oluştur
        var company = new Company
        {
            CityId = req.CityId,
            DistrictId = req.DistrictId,
            Id               = Guid.NewGuid(),
            Email            = email,
            Name             = req.CompanyName,
            Adress           = req.Adress,
            PhoneNumber      = req.PhoneNumber,
            Description      = req.Description,
            PasswordHash     = pwdHash,
            CreateDate       = DateTime.UtcNow,
            IsEmailConfirmed = false,
            IsApproved       = false
        };

        // 5) Kaydet
        await _companies.AddAsync(company);

        // 6) Daveti tüket
        await _invitations.ConsumeAsync(req.Token, company.Id, ct);

        // 7) E-posta doğrulama gönder
        await _emailVerification.GenerateAndSendAsync(company.Id, company.Email, ct);

        // 8) Başarılı dönüş
        return new GeneralResponse<RegisterByInviteResponse>
        {
            isSuccess = true,
            Message = "Kayıt başarılı. Lütfen e-posta adresinizi doğrulayın.",
            Data = new RegisterByInviteResponse
            {
                CompanyId = company.Id,
                EmailConfirmationRequired = true
            }
        };
    }

    private static GeneralResponse<RegisterByInviteResponse> Fail(string msg) => new()
    {
        isSuccess = false,
        Message = msg
    };
}
