using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;

public class GetOrganizationWithImagesQueryRequest:IRequest<GeneralResponse<GetOrganizationWithImagesResponse>>
{
    public Guid Id { get; set; }
}