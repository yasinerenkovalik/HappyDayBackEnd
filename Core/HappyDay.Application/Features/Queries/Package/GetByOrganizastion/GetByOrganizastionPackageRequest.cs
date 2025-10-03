using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Package.GetByCompany;

public class GetByOrganizastionPackageRequest: IRequest<GeneralResponse<List<GetByOrganizastionPackageResponse>>>
{
    public Guid Id { get; set; }
}