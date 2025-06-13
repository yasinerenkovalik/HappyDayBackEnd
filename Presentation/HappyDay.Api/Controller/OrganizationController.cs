using AutoMapper;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator  _mediator;
        public OrganizationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("add")]
        public async Task<GeneralResponse<CreateOrganizationCommandResponse>> AddCompany([FromForm] CreateOrganizationCommandRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
