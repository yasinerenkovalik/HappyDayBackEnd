using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HappyDay.Persistance.Security;

public class JwtService(IConfiguration configuration)
{
    public string GenerateToken(string userId, string role)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        
        var secretKeyString = jwtSettings["Secret"];
        if (string.IsNullOrEmpty(secretKeyString))
        {
            throw new ArgumentNullException("JwtSettings:Secret", "JWT Secret değeri bulunamadı!");
        }

        var secretKey = Encoding.UTF8.GetBytes(secretKeyString);
        var issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer", "JWT Issuer bulunamadı!");
        var audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience", "JWT Audience bulunamadı!");

        if (!int.TryParse(jwtSettings["ExpirationInMinutes"], out var expirationMinutes))
        {
            throw new ArgumentException("JwtSettings:ExpirationInMinutes geçerli bir sayı değil!");
        }

        var claims = new List<Claim>
        {
            new Claim("userId", userId),
            new Claim("role", role)
        };

        var key = new SymmetricSecurityKey(secretKey);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}