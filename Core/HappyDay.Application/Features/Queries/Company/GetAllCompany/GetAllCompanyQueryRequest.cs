using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Company.GetAllCompany;

public class GetAllCompanyQueryRequest:IRequest<GeneralResponse<List<GetAllCompanyQueryResponse>>>
{
    
}