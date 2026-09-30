using Microsoft.AspNetCore.Diagnostics;
using System.Threading.RateLimiting;
using System.Globalization;
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

// API her zaman nokta ondalık ayracı kullanmalı. Aksi halde sunucu kültürü tr-TR
// olduğunda form'daki "40.98426" değeri 4098426 olarak ayrıştırılıyor.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture(CultureInfo.InvariantCulture.Name);
    options.AddSupportedCultures(CultureInfo.InvariantCulture.Name);
    options.AddSupportedUICultures(CultureInfo.InvariantCulture.Name);
});

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

// CORS — izinli origin'ler config'den gelir, varsayilan olarak localhost Vite
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000", "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        if (allowedOrigins.Length == 0 || allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins).AllowAnyMethod().AllowAnyHeader();
        }
    });
});

// Rate limiting — login / mesaj gonderme gibi spam'e acik endpoint'ler icin
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("Auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("PublicWrite", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("CanCompany", policy =>
        policy.RequireRole("Admin", "Company"));
});

// Auth (JWT) — secret config'den gelir, kaynak kodda sabit tutulmaz
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSecret = jwtSection.GetValue<string>("Secret")
    ?? throw new InvalidOperationException("JwtSettings:Secret tanimli degil.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            ValidIssuer = jwtSection.GetValue<string>("Issuer"),
            ValidAudience = jwtSection.GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

// Kestrel konfigürasyonu mutlaka Build'ten ÖNCE olmalı!
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MinRequestBodyDataRate = null;
    options.Limits.MaxRequestBodySize = 15 * 1024 * 1024; // 15 MB
});

var app = builder.Build();
app.Urls.Add("http://*:8080");


// ---------------- Middleware ----------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyDay API v1");
        c.RoutePrefix = "swagger";
    });
}

// Global exception handling — yakalanmayan hatalar 500 + stack trace sizmasin
app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    context.Response.ContentType = "application/json";

    var feature = context.Features.Get<IExceptionHandlerFeature>();
    app.Logger.LogError(feature?.Error, "Islenmeyen hata: {Path}", context.Request.Path);

    await context.Response.WriteAsJsonAsync(new
    {
        isSuccess = false,
        message = "Beklenmeyen bir hata olustu. Lutfen daha sonra tekrar deneyin."
    });
}));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("DefaultCors");
app.UseRequestLocalization();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
