using AutoMapper;
using HappyDay.Application.Features.Queries.Category.GetAllCategory;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class CategoryProfile:Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, GetAllCategoryQueryResponse>().ReverseMap();
    }
}