using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetFeatured;

public class GetFeaturedQueryRequest:IRequest<GeneralResponse<List<GetFeaturedQueryResponse>>>
{
    
}