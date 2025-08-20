using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HappyDay.Api.Extension;
using HappyDay.Application;
using HappyDay.Application.Validations.Company;
using HappyDay.Persistance;
using HappyDay.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
    // To configure URLs programmatically, you can use:
    // ,Urls = { "http://0.0.0.0:80", "https://0.0.0.0:443" }
    // Note: Port 80 and 443 require elevated privileges on most systems
    // For development, consider using higher ports like:
    // ,Urls = { "http://0.0.0.0:8080", "https://0.0.0.0:8081" }
});

// Add services to the container
builder.Services.AddControllers().AddFluentValidation(conf =>
    conf.RegisterValidatorsFromAssemblyContaining<CompanyCreateValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext with connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Host=localhost;Database=HappyDayDB;Username=postgres;Password=postgres;TrustServerCertificate=True;";
builder.Services.AddDbContext<HappyDayContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddPersistanceLayerServices();
builder.Services.AddAplicationLayerServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ✅ AUTHENTICATION ve AUTHORIZATION yapılandırması burada olmalı
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BuCokGizliVeUzunBirSecretKeyOlsun1234"))
        };
    });

builder.Services.AddAuthorization();

// ✅ builder.Build en sonda çağrılmalı
var app = builder.Build();

// Middleware'leri sırayla ekle
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication(); // önce authentication
app.UseAuthorization();  // sonra authorization

app.MapControllers();

app.Run();
