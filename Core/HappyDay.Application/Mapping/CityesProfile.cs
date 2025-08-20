using AutoMapper;
using HappyDay.Application.Features.Queries.Citys.GetAllCity;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class CityesProfile:Profile
{
    public CityesProfile()
    {
        
        CreateMap<City, GetAllCityQueryResponse>();
    }
}