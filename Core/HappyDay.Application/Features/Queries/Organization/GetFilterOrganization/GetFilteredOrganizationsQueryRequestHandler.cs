using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;

public class GetFilteredOrganizationsQueryRequestHandler:IRequestHandler<GetFilteredOrganizationsQueryRequest, GeneralResponse<List<GetFilteredOrganizationsQueryResponse>>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IMapper _mapper;

    public GetFilteredOrganizationsQueryRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetFilteredOrganizationsQueryResponse>>> Handle(GetFilteredOrganizationsQueryRequest request, CancellationToken cancellationToken)
    {
        var organizations = await _repository.GetFilteredAsync(
           request);
        
        var response = _mapper.Map<List<GetFilteredOrganizationsQueryResponse>>(organizations);

        return new GeneralResponse<List<GetFilteredOrganizationsQueryResponse>>()
        {
            Data = response
        };


    }
}