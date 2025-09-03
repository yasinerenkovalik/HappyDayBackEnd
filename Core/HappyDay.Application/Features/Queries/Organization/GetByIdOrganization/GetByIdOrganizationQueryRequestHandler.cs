using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;

public class GetByIdOrganizationQueryRequestHandler:IRequestHandler<GetByIdOrganizationQueryRequest, GeneralResponse<GetByIdOrganizationQueryResponse>>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public GetByIdOrganizationQueryRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _organizationRepository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<GetByIdOrganizationQueryResponse>> Handle(GetByIdOrganizationQueryRequest request, CancellationToken cancellationToken)
    {
        var result= await _organizationRepository.GetByIdAsync(request.Id);
        if (result == null)
        {
            return new GeneralResponse<GetByIdOrganizationQueryResponse>()
            {
                Message = Messages.MessageConstants.OrganizationNotFound,
                isSuccess = false
            };
        }

        return new GeneralResponse<GetByIdOrganizationQueryResponse>()
        {
            Data = _mapper.Map<GetByIdOrganizationQueryResponse>(result),
            Message = Messages.MessageConstants.OrganizationCreated,
            isSuccess = true
        };

    }
}