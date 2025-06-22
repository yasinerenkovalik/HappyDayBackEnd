using AutoMapper;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class OrganizationProfile: Profile
{
    public OrganizationProfile()
    {
    
        CreateMap<Organization, CreateOrganizationCommandRequest>().ReverseMap();
        CreateMap<Organization, GetByIdOrganizationQueryResponse>().ReverseMap();
        CreateMap<Organization, GetAllOrganizationQueryResponse>().ReverseMap();
        CreateMap<Organization, GetByCompanyQueryResponse>().ReverseMap();

        

    }
}