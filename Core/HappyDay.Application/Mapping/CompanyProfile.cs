using AutoMapper;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.UpdateCompany;
using HappyDay.Application.Features.Queries.Company.GetByIdCompany;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class CompanyProfile: Profile
{
    public CompanyProfile()
    {
        CreateMap<Company, CreateCompanyCommandResponse>().ReverseMap();
        CreateMap<Company, CreateCompanyCommandRequest>().ReverseMap();
        CreateMap<Company, UpdateCompanyCommandResponse>().ReverseMap();
        CreateMap<Company, GetByIdCompanyQueryResponse>().ReverseMap();
    }
}