using AutoMapper;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

public class GetFilteredOrganizationsQueryRequestHandler
    : IRequestHandler<GetFilteredOrganizationsQueryRequest, GeneralResponse<PagedResult<GetFilteredOrganizationsQueryResponse>>>
{
    private readonly IOrganizationRepository _repository;

    public GetFilteredOrganizationsQueryRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _repository = repository;
    }

    public async Task<GeneralResponse<PagedResult<GetFilteredOrganizationsQueryResponse>>> Handle(
        GetFilteredOrganizationsQueryRequest request, CancellationToken cancellationToken)
    {
        var paged = await _repository.GetFilteredAsync(request,cancellationToken);

        return new GeneralResponse<PagedResult<GetFilteredOrganizationsQueryResponse>>
        {
            Data = paged,
            isSuccess = true
        };
    }
}