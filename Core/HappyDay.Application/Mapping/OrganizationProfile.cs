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

public class OrganizationProfile : Profile
{
    public OrganizationProfile()
    {
        CreateMap<Organization, CreateOrganizationCommandRequest>().ReverseMap();
        CreateMap<Organization, GetByIdOrganizationQueryResponse>().ReverseMap();

        // 🔥 GetAllOrganization için City/District mapping
        CreateMap<Organization, GetAllOrganizationQueryResponse>()
            .ForMember(dest => dest.CityName,
                opt => opt.MapFrom(src => src.City != null ? src.City.CityName : string.Empty))
            .ForMember(dest => dest.DistrictName,
                opt => opt.MapFrom(src => src.District != null ? src.District.DistrictName : string.Empty));

        CreateMap<Organization, GetByCompanyQueryResponse>().ReverseMap();
        CreateMap<Organization, UpdateOrganizationCommandRequest>().ReverseMap();

        // 🔥 Filter için de ekleyelim
        CreateMap<Organization, GetFilteredOrganizationsQueryResponse>()
            .ForMember(dest => dest.CityName,
                opt => opt.MapFrom(src => src.City != null ? src.City.CityName : string.Empty))
            .ForMember(dest => dest.DistrictName,
                opt => opt.MapFrom(src => src.District != null ? src.District.DistrictName : string.Empty));

        CreateMap<Organization, GetFeaturedQueryResponse>().ReverseMap();
    }
}