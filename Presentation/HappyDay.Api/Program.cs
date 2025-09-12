using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HappyDay.Api.Services.Mail;
using HappyDay.Application;
using HappyDay.Application.Common.Security;
using HappyDay.Application.Validations.Company;
using HappyDay.Persistance;
using HappyDay.Persistance.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
});

// ---------------- Services ----------------
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CompanyCreateValidator>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<
    HappyDay.Application.Common.Email.IEmailSender,
    HappyDay.Api.Services.Mail.MailService
>(); 
// Program.cs
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));


// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HappyDayContext>(options =>
    options.UseNpgsql(connectionString));

// Katman servisleri
builder.Services.AddPersistanceLayerServices();
builder.Services.AddAplicationLayerServices();

// Password hashing
var security = builder.Configuration.GetSection("Security");
var workFactor = security.GetValue<int?>("PasswordWorkFactor") ?? 12;
var pepper = security.GetValue<string>("Pepper");

builder.Services.AddSingleton<IPasswordHasher>(
    _ => new BcryptPasswordHasher(workFactor, pepper)
);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("CanCompany", policy =>
        policy.RequireRole("Admin", "Company"));
});

// Auth (JWT)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("BuCokGizliVeUzunBirSecretKeyOlsun1234"))
        };
    });

// Kestrel konfigürasyonu mutlaka Build'ten ÖNCE olmalı!
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MinRequestBodyDataRate = null;
});

var app = builder.Build();


// ---------------- Middleware ----------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyDay API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
