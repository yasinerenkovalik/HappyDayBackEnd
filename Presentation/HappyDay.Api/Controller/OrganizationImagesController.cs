using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Commands.OrganizationImages.CreateOrganizationImages;
using HappyDay.Application.Features.Commands.OrganizationImages.DeleteOrganizationImages;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("AddOrganizationImages")]
        public async Task<GeneralResponse<CreateOrganizationImagesCommandResponse>> AddOrganizationImages([FromForm] CreateOrganizationImagesCommandRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpDelete("DeleteOrganizationImages/{id}")]
        public async Task<GeneralResponse<DeleteOrganizationImagesCommandResponse>> DeleteOrganizationImages(int id)
        {
            var request = new DeleteOrganizationImagesCommandRequest { Id = id };
            return await _mediator.Send(request);
        }
    }
}
