using AutoMapper;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetByCompany;

public class GetByCompanyQueryRequestHandler:IRequestHandler<GetByCompanyQueryRequest,GeneralResponse<List<GetByCompanyQueryResponse>>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IMapper _mapper;

    public GetByCompanyQueryRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public  async Task<GeneralResponse<List<GetByCompanyQueryResponse>>> Handle(GetByCompanyQueryRequest request, CancellationToken cancellationToken)
    {
        var response= await _repository.GetByCompany(request.CompanyId);
        var mappedResponse = _mapper.Map<List<GetByCompanyQueryResponse>>(response);
        return new GeneralResponse<List<GetByCompanyQueryResponse>>()
        {
            Data = mappedResponse
        };
    }
}

