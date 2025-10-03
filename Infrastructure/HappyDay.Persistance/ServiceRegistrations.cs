
using HappyDay.Api.Services.Mail;
using HappyDay.Application.Features.EmailVerification;
using HappyDay.Application.Features.Invitations;
using HappyDay.Application.Features.PasswordReset;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Repositories;
using AppEmail = HappyDay.Application.Common.Email.IEmailSender; 

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
        services.AddScoped<IOrganizationImagesRepository, OrganizationImagesRepository>();
        services.AddScoped<ICityesRepository, CityesRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
        services.AddScoped<ICompanyInvitationService, CompanyInvitationService>();
        services.AddScoped<ICompanyInvitationRepository, CompanyInvitationRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IEmailVerificationService, EmailVerificationService>();
        services.AddScoped<IPackageRepository, PackageRepository>();
        services.AddScoped<ICalanderEvenetRepository, CalanderRepository>();
        
        return services;
    }
}