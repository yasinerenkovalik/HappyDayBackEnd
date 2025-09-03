using AutoMapper;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Commands.Organization.UpdateOrganization;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Application.Features.Queries.Organization.GetFeatured;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
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
        CreateMap<Organization, UpdateOrganizationCommandRequest>().ReverseMap();
        CreateMap<Organization, GetFilteredOrganizationsQueryResponse>().ReverseMap();
        CreateMap<Organization, GetFeaturedQueryResponse>().ReverseMap();
       
    }
}