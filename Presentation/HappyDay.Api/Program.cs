using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HappyDay.Application;
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

// FluentValidation (önerilen kullanım)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CompanyCreateValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (conn string appsettings.json → ConnectionStrings:DefaultConnection)
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");
   

builder.Services.AddDbContext<HappyDayContext>(options =>
    options.UseNpgsql(connectionString));

// Katman servisleri
builder.Services.AddPersistanceLayerServices();
builder.Services.AddAplicationLayerServices();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});
builder.Services.AddAuthorization(options =>
{
    
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    // hem Admin hem Manager girebilir
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


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MinRequestBodyDataRate = null;
});


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyDay API v1");
    c.RoutePrefix = "swagger";
});

// Not: Sertifika yoksa container’da HTTPS yönlendirmeyi geçici kapatabilirsin.
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
