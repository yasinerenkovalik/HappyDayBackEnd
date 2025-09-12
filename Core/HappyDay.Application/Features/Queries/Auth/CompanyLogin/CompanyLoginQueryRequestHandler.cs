using System.Text.RegularExpressions;
using HappyDay.Application.Common.Security;
using HappyDay.Application.Features.Queries.Auth.OrganizationLogin; // IPasswordHasher, BcryptPasswordHasher
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;
using MediatR;
using Microsoft.Extensions.Configuration;

public class CompanyLoginQueryRequestHandler
    : IRequestHandler<CompanyLoginQueryRequest, GeneralResponse<CompanyLoginQueryResponse>>
{
    private readonly ICompanyRepository _companies;
    private readonly JwtService _jwt;
    private readonly BcryptPasswordHasher _pepperedHasher;
    private readonly BcryptPasswordHasher _noPepperHasher;

    // bcrypt format kontrolü
    private static bool LooksLikeBcrypt(string? s) =>
        !string.IsNullOrWhiteSpace(s) &&
        Regex.IsMatch(s, @"^\$2[aby]\$\d{2}\$[./A-Za-z0-9]{53}$");

    public CompanyLoginQueryRequestHandler(
        ICompanyRepository companies,
        JwtService jwt,
        IConfiguration config)
    {
        _companies = companies;
        _jwt = jwt;

        var sec = config.GetSection("Security");
        var work   = sec.GetValue<int?>("PasswordWorkFactor") ?? 12;
        var pepper = sec.GetValue<string>("Pepper"); // null olabilir → no-pepper doğrulaması da var

        _pepperedHasher = new BcryptPasswordHasher(work, pepper);
        _noPepperHasher = new BcryptPasswordHasher(work, null);
    }

    public async Task<GeneralResponse<CompanyLoginQueryResponse>> Handle(
        CompanyLoginQueryRequest request, CancellationToken ct)
    {
        // 0) Model/body kontrol (binding sorunlarını yakalar)
        if (request is null ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Fail("Geçersiz istek: e-posta ve şifre zorunludur.");
        }

        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        var input = request.Password ?? string.Empty;

        // 1) Kullanıcıyı çek
        var company = await _companies.GetByEmailAsync(email);
        if (company is null)
        {
            // Bilgi sızdırmamak için genel mesaj
            return Fail("E-posta veya şifre hatalı.");
        }

        if (string.IsNullOrWhiteSpace(company.PasswordHash))
        {
            return Fail("E-posta veya şifre hatalı.");
        }

        // 2) Şifre doğrulaması
        var stored = company.PasswordHash;
        bool ok = false;

        if (LooksLikeBcrypt(stored))
        {
            try
            {
                // önce pepper'lı deneriz
                ok = _pepperedHasher.Verify(input, stored);

                // olmazsa pepper'sız hash ile de dene (eski kayıtlar için)
                if (!ok)
                {
                    ok = _noPepperHasher.Verify(input, stored);

                    // pepper'sız başarıysa → hash’i pepper’lıya yükselt
                    if (ok)
                    {
                        company.PasswordHash = _pepperedHasher.Hash(input);
                        await _companies.UpdateAsync(company);
                    }
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                ok = false; // bozuk bcrypt formatı
            }
        }
        else
        {
            // çok eski/düz metin dönemi
            ok = stored == input;
            if (ok)
            {
                // ilk başarılı girişte bcrypt+pepper'a yükselt
                company.PasswordHash = _pepperedHasher.Hash(input);
                await _companies.UpdateAsync(company);
            }
        }

        if (!ok)
        {
            return Fail("E-posta veya şifre hatalı.");
        }

        // 3) (İstersen burada e-posta/admin onayı kontrolü ekleyebilirsin)
        // if (!company.IsEmailConfirmed) return Fail("E-posta adresinizi doğrulamanız gerekiyor.");
        // if (!company.IsApproved)      return Fail("Hesabınız henüz admin tarafından onaylanmamış.");

        // 4) Token üret
        // Not: Mevcut imzanı bozmamak için company.Id iki kez gönderildi (senin kodundaki gibi).
        // Eğer GenerateCompanyToken(userId, companyId) ise ilk parametre userId olmalı.
        var token = _jwt.GenerateCompanyToken(company.Id.ToString(), company.Id.ToString());

        return new GeneralResponse<CompanyLoginQueryResponse>
        {
            isSuccess = true,
            Message   = "Giriş başarılı.",
            Data      = new CompanyLoginQueryResponse
            {
                Token = token,
                IsEmailConfirmed = company.IsEmailConfirmed
            }
        };
    }

    // Tek yerden, tutarlı hata mesaji
    private static GeneralResponse<CompanyLoginQueryResponse> Fail(string msg) => new()
    {
        isSuccess = false,
        Message   = msg,
        Data      = null
    };
}
