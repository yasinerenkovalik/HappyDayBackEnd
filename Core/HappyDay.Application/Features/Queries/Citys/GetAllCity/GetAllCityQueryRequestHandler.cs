using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Citys.GetAllCity;

public class GetAllCityQueryRequestHandler:IRequestHandler<GetAllCityQueryRequest, GeneralResponse<List<GetAllCityQueryResponse>>>
{
    private readonly ICityesRepository  _cityesRepository;
    private readonly IMapper _mapper;

    public GetAllCityQueryRequestHandler(ICityesRepository cityesRepository, IMapper mapper)
    {
        _cityesRepository = cityesRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetAllCityQueryResponse>>> Handle(GetAllCityQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _cityesRepository.GetAllAysnc();
        
        return new GeneralResponse<List<GetAllCityQueryResponse>>()
        {
            Data = _mapper.Map<List<GetAllCityQueryResponse>>(result)
        };
    }
}