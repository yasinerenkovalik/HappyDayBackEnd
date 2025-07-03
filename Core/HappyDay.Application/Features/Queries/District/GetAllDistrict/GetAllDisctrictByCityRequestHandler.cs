using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.District.GetAllDistrict;

public class GetAllDisctrictByCityRequestHandler:IRequestHandler<GetAllDisctrictByCityRequest,GeneralResponse<List<GetAllDisctrictByCityResponse>>>
{
    private readonly IMapper _mapper;
    private readonly IDistrictRepository _districtRepository;

    public GetAllDisctrictByCityRequestHandler(IMapper mapper, IDistrictRepository districtRepository)
    {
        _mapper = mapper;
        _districtRepository = districtRepository;
    }

    public async Task<GeneralResponse<List<GetAllDisctrictByCityResponse>>> Handle(GetAllDisctrictByCityRequest request, CancellationToken cancellationToken)
    {
        var result= await _districtRepository.GetDistrictsByCityIdAsync(request.CityId);
        return new GeneralResponse<List<GetAllDisctrictByCityResponse>>()
        {
            Data = _mapper.Map<List<GetAllDisctrictByCityResponse>>(result)
        };

    }
}