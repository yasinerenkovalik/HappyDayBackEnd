using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Contact.GetByIdContact;

public class GetByIdContactQueryRequest:IRequest<GeneralResponse<GetByIdContactQueryResponse>>
{
    public Guid Id { get; set; }
}