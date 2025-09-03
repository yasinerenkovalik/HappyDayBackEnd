using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryRequest:IRequest<GeneralResponse<List<GetAllOrganizationQueryResponse>>>
{
    
}