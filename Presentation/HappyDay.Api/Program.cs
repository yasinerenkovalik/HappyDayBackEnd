using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using HappyDay.Api.Extension;
using HappyDay.Application;
using HappyDay.Application.Validations.Company;
using HappyDay.Persistance;
using HappyDay.Persistance.Context;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot"
});

// Add services to the container
builder.Services.AddControllers().AddFluentValidation(conf =>
    conf.RegisterValidatorsFromAssemblyContaining<CompanyCreateValidator>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HappyDayContext>();

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
