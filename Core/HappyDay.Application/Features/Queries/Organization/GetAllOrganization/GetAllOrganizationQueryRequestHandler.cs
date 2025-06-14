using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryRequestHandler:IRequestHandler<GetAllOrganizationQueryRequest, GeneralResponse<List<GetAllOrganizationQueryResponse>>>
{
    private readonly IOrganizationRepository  _organizationRepository;
    private readonly IMapper _mapper;

    public GetAllOrganizationQueryRequestHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<List<GetAllOrganizationQueryResponse>>> Handle(GetAllOrganizationQueryRequest request, CancellationToken cancellationToken)
    {
        var result= await _organizationRepository.GetAllAysnc();
        if (result == null)
        {
            return new GeneralResponse<List<GetAllOrganizationQueryResponse>>()
            {
                Message = Messages.MessageConstants.OrganizationNotFound,
                isSuccess = false
            };
        }
        var organization = _mapper.Map<List<GetAllOrganizationQueryResponse>>(result);
        return new GeneralResponse<List<GetAllOrganizationQueryResponse>>()
        {
            Message = Messages.MessageConstants.OrganizationNotFound,
            isSuccess = false,
            Data = organization
        };
    }
}