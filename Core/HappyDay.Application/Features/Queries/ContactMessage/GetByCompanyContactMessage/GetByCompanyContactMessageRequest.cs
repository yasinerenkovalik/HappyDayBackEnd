using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;

public class GetByCompanyContactMessageRequest:IRequest<GeneralResponse<List<GetByCompanyContactMessageResponse>>>
{
    public Guid CompanyId { get; set; }
}