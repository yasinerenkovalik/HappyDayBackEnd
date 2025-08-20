using AutoMapper;
using HappyDay.Application.Features.Queries.District.GetAllDistrict;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class DistrictProfile:Profile
{
    public DistrictProfile()
    {
        CreateMap<District, GetAllDisctrictByCityResponse>().ReverseMap();
    }
}