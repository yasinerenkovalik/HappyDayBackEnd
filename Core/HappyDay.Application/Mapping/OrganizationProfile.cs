using AutoMapper;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class OrganizationProfile: Profile
{
    public OrganizationProfile()
    {
    
        CreateMap<Organization, CreateOrganizationCommandRequest>().ReverseMap();
    }
}