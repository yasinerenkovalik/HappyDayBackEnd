using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.OrganizationImages.DeleteOrganizationImages;

public class DeleteOrganizationImagesCommandRequestHandler:IRequestHandler<DeleteOrganizationImagesCommandRequest, GeneralResponse<DeleteOrganizationImagesCommandResponse>>
{
    private readonly IOrganizationImagesRepository  _repository;

    public DeleteOrganizationImagesCommandRequestHandler(IOrganizationImagesRepository repository)
    {
        _repository = repository;
    }

    public async Task<GeneralResponse<DeleteOrganizationImagesCommandResponse>> Handle(DeleteOrganizationImagesCommandRequest request, CancellationToken cancellationToken)
    {
      await  _repository.DeleteOrganizationImageAsync(request.Id);
        return new GeneralResponse<DeleteOrganizationImagesCommandResponse>
        {
            Message = "Organization image deleted",
            Data = new DeleteOrganizationImagesCommandResponse {}
        };
    }
}