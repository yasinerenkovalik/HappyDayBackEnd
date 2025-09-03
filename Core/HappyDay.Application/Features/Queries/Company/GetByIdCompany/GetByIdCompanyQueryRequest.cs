using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Company.GetByIdCompany;

public class GetByIdCompanyQueryRequest:IRequest<GeneralResponse<GetByIdCompanyQueryResponse>>
{
  public Guid Id { get; set; } 
}