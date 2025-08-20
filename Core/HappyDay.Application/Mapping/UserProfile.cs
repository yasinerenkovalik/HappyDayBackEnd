using AutoMapper;
using HappyDay.Application.Features.Commands.User.CreateUser;
using HappyDay.Application.Features.Queries.User.GetAllUser;
using HappyDay.Application.Features.Queries.User.GetByIdUser;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Mapping;

public class UserProfile:Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommandRequest, User>().ReverseMap();
        CreateMap<GetAllUserQueryResponse, User>().ReverseMap();
        CreateMap<GetByIdUserQueryResponse, User>().ReverseMap();
    }
}