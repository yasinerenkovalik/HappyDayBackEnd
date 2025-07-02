using AutoMapper;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Commands.Organization.UpdateOrganization;
using HappyDay.Application.Features.Queries.Company.GetByIdCompany;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

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
        [HttpPost("AddOrganization")]
        public async Task<GeneralResponse<CreateOrganizationCommandResponse>> AddOrganization([FromForm] CreateOrganizationCommandRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpGet("OrganizationGetById")]
        public async Task<GeneralResponse<GetByIdOrganizationQueryResponse>> OrganizationGetById([FromQuery] Guid id)
        {
            var request = new GetByIdOrganizationQueryRequest { Id = id };
            return await _mediator.Send(request);
        }
        [HttpGet("OrganizationGetAll")]
        public async Task<GeneralResponse<List<GetAllOrganizationQueryResponse>>> OrganizationGetAll()
        {
            
            return await _mediator.Send(new GetAllOrganizationQueryRequest());
        }
  
        [HttpGet("GetOrganizationWithImages")]
        public async Task<GeneralResponse<GetOrganizationWithImagesResponse>> GetOrganizationWithImages(Guid Id)
        {
            
            return await _mediator.Send(new GetOrganizationWithImagesQueryRequest { Id = Id });
        }
        [HttpGet("GetOrganizationWithICompany")]
        public async Task<GeneralResponse<List<GetByCompanyQueryResponse>>> GetOrganizationWithICompany(Guid Id)
        {
            
            return await _mediator.Send(new GetByCompanyQueryRequest() { CompanyId = Id });
        }
        [HttpPut("OrganizationUpdate")]
        public async Task<GeneralResponse<UpdateOrganizationCommandResponse>> OrganizationUpdate([FromForm] UpdateOrganizationCommandRequest request)
        {
            
            return await _mediator.Send(request);
        }
        [HttpGet("Filter")]
        public async Task<GeneralResponse<GetFilteredOrganizationsQueryResponse>>GetFiltered([FromQuery] GetFilteredOrganizationsQueryRequest query)
        {
            return await _mediator.Send(query);
            
        }
    }
}
