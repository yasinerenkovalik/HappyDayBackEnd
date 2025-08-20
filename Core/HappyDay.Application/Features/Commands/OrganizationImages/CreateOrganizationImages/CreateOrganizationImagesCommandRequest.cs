using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HappyDay.Application.Features.Commands.OrganizationImages.CreateOrganizationImages;

public class CreateOrganizationImagesCommandRequest:IRequest<GeneralResponse<CreateOrganizationImagesCommandResponse>>
{
    public Guid OrganizationId { get; set; }
    public IFormFile OrganizationImage { get; set; }
}