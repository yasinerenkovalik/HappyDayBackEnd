using HappyDay.Application.Common.Security;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Persistance.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace HappyDay.Application.Features.Queries.Auth.PlatformLogin;

public class PlatformLoginQueryRequestHandler
    : IRequestHandler<PlatformLoginQueryRequest, GeneralResponse<PlatformLoginQueryResponse>>
{
    private readonly IUserRepository _users;
    private readonly ICompanyRepository _companies;
    private readonly JwtService _jwt;
    private readonly BcryptPasswordHasher _pepperedHasher;
    private readonly BcryptPasswordHasher _noPepperHasher;

    private static readonly Regex BcryptPattern =
        new(@"^\$2[aby]\$\d{2}\$[./A-Za-z0-9]{53}$", RegexOptions.Compiled);

    public PlatformLoginQueryRequestHandler(
        IUserRepository users,
        ICompanyRepository companies,
        JwtService jwt,
        IConfiguration config)
    {
        _users = users;
        _companies = companies;
        _jwt = jwt;

        var security = config.GetSection("Security");
        var workFactor = security.GetValue<int?>("PasswordWorkFactor") ?? 12;
        var pepper = security.GetValue<string>("Pepper");

        _pepperedHasher = new BcryptPasswordHasher(workFactor, pepper);
        _noPepperHasher = new BcryptPasswordHasher(workFactor, null);
    }

    public async Task<GeneralResponse<PlatformLoginQueryResponse>> Handle(
        PlatformLoginQueryRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Fail("E-posta ve şifre zorunludur.");
        }

        var email = request.Email.Trim().ToLowerInvariant();
        var password = request.Password;

        // 1) Kullanıcı hesabı (admin / normal kullanıcı)
        // GetByEmailAsync pasif kayıtları filtreler — pasif hesap da "şifre hatalı" görünür.
        var user = await _users.GetByEmailAsync(email);

        if (user is not null)
        {
            if (!VerifyPassword(password, user.PasswordHash))
            {
                return Fail("E-posta veya şifre hatalı.");
            }

            var isAdmin = string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase);

            // Admin token'ı yalnızca burada üretilir.
            var token = isAdmin
                ? _jwt.GenerateAdminToken(user.Id.ToString())
                : _jwt.GenerateUserToken(
                    user.Id.ToString(),
                    user.CompanyId.HasValue && user.CompanyId.Value != Guid.Empty
                        ? user.CompanyId.Value.ToString()
                        : null);

            return Success(token, isAdmin ? "Admin" : "User");
        }

        // 2) Firma hesabı
        var company = await _companies.GetByEmailAsync(email);

        if (company is not null)
        {
            if (!company.IsActivated)
            {
                return Fail("Bu firma hesabı pasif durumda.");
            }

            if (!VerifyPassword(password, company.PasswordHash))
            {
                return Fail("E-posta veya şifre hatalı.");
            }

            var token = _jwt.GenerateCompanyToken(company.Id.ToString(), company.Id.ToString());
            return Success(token, "Company");
        }

        // Kullanıcı/şirket ayrımı yapılmaz — bilgi sızdırmamak için.
        return Fail("E-posta veya şifre hatalı.");
    }

    /// <summary>Hem pepper'lı hem pepper'sız (eski kayıtlar) hash'leri dener.</summary>
    private bool VerifyPassword(string input, string? storedHash)
    {
        if (string.IsNullOrWhiteSpace(storedHash) || !BcryptPattern.IsMatch(storedHash))
        {
            return false;
        }

        if (_pepperedHasher.Verify(input, storedHash))
        {
            return true;
        }

        return _noPepperHasher.Verify(input, storedHash);
    }

    private static GeneralResponse<PlatformLoginQueryResponse> Success(string token, string role) =>
        new()
        {
            Data = new PlatformLoginQueryResponse { Token = token, Role = role },
            isSuccess = true,
            Message = "Giriş başarılı."
        };

    private static GeneralResponse<PlatformLoginQueryResponse> Fail(string message) =>
        new()
        {
            Data = null,
            isSuccess = false,
            Message = message
        };
}
