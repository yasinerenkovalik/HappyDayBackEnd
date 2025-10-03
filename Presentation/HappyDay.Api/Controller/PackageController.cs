using HappyDay.Application.Features.Commands.Package.CreatePackage;
using HappyDay.Application.Features.Commands.Package.UpdatePackage;
using HappyDay.Application.Features.Commands.Package.DeletePackage;
using HappyDay.Application.Features.Queries.Package.GetByCompany;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PackageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // CREATE
        [HttpPost("create")]
        public async Task<GeneralResponse<CreatePackageCommandResponse>> Create([FromBody] CreatePackageCommandRequest request)
        {
            return await _mediator.Send(request);
        }

        // GET BY ORGANIZATION
        [HttpPost("get-by-organization")]
        public async Task<GeneralResponse<List<GetByOrganizastionPackageResponse>>> GetByOrganization([FromBody] GetByOrganizastionPackageRequest request)
        {
            return await _mediator.Send(request);
        }

        // UPDATE
        [HttpPut("update")]
        public async Task<GeneralResponse<UpdatePackageCommandResponse>> Update([FromBody] UpdatePackageCommandRequest request)
        {
            return await _mediator.Send(request);
        }

        // DELETE
        [HttpDelete("delete/{id:guid}")]
        public async Task<GeneralResponse<DeletePackageCommandResponse>> Delete(Guid id)
        {
            var request = new DeletePackageCommandRequest { Id = id };
            return await _mediator.Send(request);
        }
    }
}