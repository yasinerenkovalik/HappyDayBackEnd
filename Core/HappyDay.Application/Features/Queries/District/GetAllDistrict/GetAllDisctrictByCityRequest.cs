using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.District.GetAllDistrict;

public class GetAllDisctrictByCityRequest:IRequest<GeneralResponse<List<GetAllDisctrictByCityResponse>>>
{
    public int CityId { get; set; }
}