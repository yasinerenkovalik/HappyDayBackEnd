using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Contact.GetAllContact;

public class GetAllContactQueryRequest:IRequest<GeneralResponse<List<GetAllContactQueryResponse>>>
{
    
}