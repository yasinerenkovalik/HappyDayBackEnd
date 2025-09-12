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
    private static bool LooksLikeBcrypt(string? s)
        => !string.IsNullOrWhiteSpace(s) 
           && Regex.IsMatch(s, @"^\$2[aby]\$\d{2}\$[./A-Za-z0-9]{53}$");

    public CompanyLoginQueryRequestHandler(
        ICompanyRepository companies,
        JwtService jwt,
        // DI’den tek hasher geçiyorsan pepper bilgisini config’ten alıp burada 2 instance oluştur.
        IConfiguration config)
    {
        _companies = companies;
        _jwt = jwt;

        var sec = config.GetSection("Security");
        var work = sec.GetValue<int?>("PasswordWorkFactor") ?? 12;
        var pepper = sec.GetValue<string>("Pepper");

        _pepperedHasher = new BcryptPasswordHasher(work, pepper);
        _noPepperHasher = new BcryptPasswordHasher(work, null);
    }

    public async Task<GeneralResponse<CompanyLoginQueryResponse>> Handle(
        CompanyLoginQueryRequest request, CancellationToken ct)
    {
        
        var email = (request.Email ?? "").Trim().ToLowerInvariant();
        var company = await _companies.GetByEmailAsync(email);
        if (company is null || string.IsNullOrWhiteSpace(company.PasswordHash))
            return Fail();

        var stored = company.PasswordHash;
        var input = request.Password ?? string.Empty;

        bool ok;
        if (LooksLikeBcrypt(stored))
        {
            try
            {
           
                ok = _pepperedHasher.Verify(input, stored);
                if (!ok)
                {
                  
                    ok = _noPepperHasher.Verify(input, stored);
                    if (ok)
                    {
                        // Upgrade: pepper'lı hash'e çevir
                        company.PasswordHash = _pepperedHasher.Hash(input);
                        await _companies.UpdateAsync(company);
                    }
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                ok = false; // bozuk format
            }
        }
        else
        {
            // Düz metin dönemi
            ok = stored == input;
            if (ok)
            {
                company.PasswordHash = _pepperedHasher.Hash(input);
                await _companies.UpdateAsync(company);
            }
        }

        if (!ok) return Fail();

        var token = _jwt.GenerateCompanyToken(company.Id.ToString(), company.Id.ToString());
        return new GeneralResponse<CompanyLoginQueryResponse>
        {
           
            Data = new CompanyLoginQueryResponse { Token = token ,IsEmailConfirmed=company.IsEmailConfirmed},
            isSuccess = true
        };

        static GeneralResponse<CompanyLoginQueryResponse> Fail() => new()
        {
          
            isSuccess = false
        };
    }
}


