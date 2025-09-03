using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.User.GetAllUser;

public class GetAllUserQueryRequest:IRequest<GeneralResponse<List<GetAllUserQueryResponse>>>
{
    
}