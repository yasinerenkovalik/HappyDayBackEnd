using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.OrganizationImages.DeleteOrganizationImages;

public class DeleteOrganizationImagesCommandRequest:IRequest<GeneralResponse<DeleteOrganizationImagesCommandResponse>>
{
    public int Id { get; set; }
}