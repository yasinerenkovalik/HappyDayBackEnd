using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryRequest:IRequest<GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}