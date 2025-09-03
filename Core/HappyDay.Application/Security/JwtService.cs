using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HappyDay.Persistance.Security;

public class JwtService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _secret;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        var jwt = configuration.GetSection("JwtSettings");
        _secret = jwt["Secret"] ?? throw new ArgumentNullException("JwtSettings:Secret");
        _issuer = jwt["Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
        _audience = jwt["Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience");
        if (!int.TryParse(jwt["ExpirationInMinutes"], out _expirationMinutes))
            throw new ArgumentException("JwtSettings:ExpirationInMinutes geçersiz.");
    }

    // 1) ADMIN için sade fonksiyon
    public string GenerateAdminToken(string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, "Admin")
        };
        return BuildToken(claims);
    }

    // 2) COMPANY için ayrı fonksiyon (companyId zorunlu)
    public string GenerateCompanyToken(string userId, string companyId)
    {
        if (!Guid.TryParse(companyId, out _))
            throw new ArgumentException("companyId geçerli bir GUID olmalı.", nameof(companyId));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId), // nameid
            new(ClaimTypes.Role, "Company"),
            new("CompanyId", companyId)             // özel claim
        };
        return BuildToken(claims);
    }
    public string GenerateUserToken(string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, "Admin"),
            new("CompanyId", userId)   
        };
        return BuildToken(claims);
    }

    // Ortak token üretimi
    private string BuildToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = credentials
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
