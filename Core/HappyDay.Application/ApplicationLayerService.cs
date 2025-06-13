using FluentValidation;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Interface.Services;
using HappyDay.Application.Validations.Company;
using HappyDay.Persistance.Security;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HappyDay.Application;

public static class ApplicationLayerService
{
    public static IServiceCollection AddAplicationLayerServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(ApplicationLayerService).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(currentAssembly));

        services.AddAutoMapper(currentAssembly);

        // Tüm validator'ları otomatik olarak ekle
        services.AddValidatorsFromAssembly(currentAssembly);
        services.AddScoped<JwtService>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}