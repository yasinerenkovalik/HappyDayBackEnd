using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;

public class GetFilteredOrganizationsQueryRequest:IRequest<GeneralResponse<List<GetFilteredOrganizationsQueryResponse>>>
{
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsOutdoor { get; set; }
    public decimal? MaxPrice { get; set; }
}