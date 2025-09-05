using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetAllOrganization;

public class GetAllOrganizationQueryRequestHandler 
    : IRequestHandler<GetAllOrganizationQueryRequest, GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IMapper _mapper;

    public GetAllOrganizationQueryRequestHandler(IOrganizationRepository organizationRepository, IMapper mapper)
    {
        _organizationRepository = organizationRepository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>> Handle(GetAllOrganizationQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _organizationRepository.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);

        if (result.Items == null || !result.Items.Any())
        {
            return new GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>
            {
                Message = Messages.MessageConstants.OrganizationNotFound,
                isSuccess = false
            };
        }

        var mappedItems = _mapper.Map<List<GetAllOrganizationQueryResponse>>(result.Items);

        var pagedResponse = new PagedResult<GetAllOrganizationQueryResponse>
        {
            Items = mappedItems,
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };

        return new GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>
        {
            Message = Messages.MessageConstants.OrganizationNotFound,
            isSuccess = true,
            Data = pagedResponse
        };
    }
}