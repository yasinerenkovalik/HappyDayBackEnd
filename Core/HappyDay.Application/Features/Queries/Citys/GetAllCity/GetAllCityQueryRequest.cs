using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Citys.GetAllCity;

public class GetAllCityQueryRequest:IRequest<GeneralResponse<List<GetAllCityQueryResponse>>>
{
    
}