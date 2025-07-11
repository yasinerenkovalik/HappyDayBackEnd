using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetFeatured;

public class GetFeaturedQueryRequestHandler:IRequestHandler<GetFeaturedQueryRequest, GeneralResponse<List<GetFeaturedQueryResponse>>>
{
    private readonly IOrganizationRepository  _organizationRepository;
    private readonly IMapper              _mapper;

    public GetFeaturedQueryRequestHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public  async Task<GeneralResponse<List<GetFeaturedQueryResponse>>> Handle(GetFeaturedQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetFeaturedAsync();
       
        return new GeneralResponse<List<GetFeaturedQueryResponse>>()
        {
            Data = _mapper.Map<List<GetFeaturedQueryResponse>>(result),
          
            isSuccess = true
        };
    }
}