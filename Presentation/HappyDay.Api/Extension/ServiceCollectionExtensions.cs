using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HappyDay.Api.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];

            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("JwtSettings:Secret", "JWT Secret değeri bulunamadı!");

            if (secretKey.Length < 32)
                throw new ArgumentException("JWT Secret Key en az 32 karakter olmalıdır!");

            var key = Encoding.UTF8.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
                options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
            });

            return services;
        }
    }
}
