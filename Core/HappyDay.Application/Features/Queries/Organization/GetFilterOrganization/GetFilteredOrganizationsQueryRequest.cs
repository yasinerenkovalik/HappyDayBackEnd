// GetFilteredOrganizationsQueryRequest.cs

using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using MediatR;
using HappyDay.Application.Wrappers;

public class GetFilteredOrganizationsQueryRequest 
    : IRequest<GeneralResponse<PagedResult<GetFilteredOrganizationsQueryResponse>>>
{
    // mevcut filtrelerin
    public int? CityId { get; set; }
    public int? DistrictId { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsOutdoor { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? SortByPriceAsc { get; set; }

    // pagination
    private const int MaxPageSize = 100;
    public int Page { get; set; } = 1;

    private int _pageSize = 24;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? 24 : (value > MaxPageSize ? MaxPageSize : value);
    }
}