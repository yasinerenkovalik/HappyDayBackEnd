using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.User.GetByIdUser;

public class GetByIdUserQueryRequest:IRequest<GeneralResponse<GetByIdUserQueryResponse>>
{
    public Guid Id { get; set; }
}