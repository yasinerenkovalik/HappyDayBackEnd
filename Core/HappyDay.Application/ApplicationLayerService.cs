using FluentValidation;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Validations.Company;
using Microsoft.Extensions.DependencyInjection;

namespace HappyDay.Application;

public static class ApplicationLayerService
{
    public static IServiceCollection AddAplicationLayerServices(this IServiceCollection services)
    {
        var currentAssembly = typeof(ApplicationLayerService).Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(currentAssembly);
        });

        services.AddAutoMapper(currentAssembly);
        services.AddScoped<IValidator<CreateCompanyCommandRequest>, CompanyCreateValidator>();
        return services;
    }
}