using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;

public class GetByIdOrganizationQueryRequest:IRequest<GeneralResponse<GetByIdOrganizationQueryResponse>>
{
    public Guid Id { get; set; }
}