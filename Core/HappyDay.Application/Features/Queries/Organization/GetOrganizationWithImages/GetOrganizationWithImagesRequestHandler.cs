using AutoMapper;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;

public class GetOrganizationWithImagesRequestHandler:IRequestHandler<GetOrganizationWithImagesQueryRequest, GeneralResponse<GetOrganizationWithImagesResponse>>
{
    private readonly IOrganizationRepository _repository;
    private readonly IMapper _mapper;

    public GetOrganizationWithImagesRequestHandler(IOrganizationRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GeneralResponse<GetOrganizationWithImagesResponse>> Handle(GetOrganizationWithImagesQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetOrganizationWithImages(request.Id);
        if (result == null)
        {
            return new GeneralResponse<GetOrganizationWithImagesResponse>()
            {
                Message = Messages.MessageConstants.OrganizationNotFound,
                isSuccess = false
            };
        }
        return new GeneralResponse<GetOrganizationWithImagesResponse>()
        {
            Message = Messages.MessageConstants.OrganizationGet,
            isSuccess = true,
            Data = result
        };
    }
}