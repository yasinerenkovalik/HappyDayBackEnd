using AutoMapper;
using HappyDay.Application.Features.Commands.OrganizationImages.CreateOrganizationImages;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class OrganizationImagesProfile:Profile
{
    public OrganizationImagesProfile()
    {
        
        CreateMap<OrganizationImage, CreateOrganizationImagesCommandRequest>().ReverseMap();
        CreateMap<OrganizationImage, CreateOrganizationImagesCommandRequest>().ReverseMap();
    }
}