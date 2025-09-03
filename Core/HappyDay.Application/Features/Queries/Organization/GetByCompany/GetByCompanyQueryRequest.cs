using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetByCompany;

public class GetByCompanyQueryRequest:IRequest<GeneralResponse<List<GetByCompanyQueryResponse>>>
{
    public Guid CompanyId { get; set; }
}