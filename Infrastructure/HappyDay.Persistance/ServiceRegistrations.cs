
using HappyDay.Application.Interface.Repository;
using HappyDay.Persistance.Context;
using HappyDay.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HappyDay.Persistance;

public static class ServiceRegistrations
{
    public static IServiceCollection AddPersistanceLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
       
        return services;
    }
}