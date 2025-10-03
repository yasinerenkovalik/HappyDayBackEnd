using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Commands.Organization.DeleteOrganization;
using HappyDay.Application.Features.Commands.Organization.UpdateOrganization;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Features.Queries.Organization.GetByCompany;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Application.Features.Queries.Organization.GetFeatured;
using HappyDay.Application.Features.Queries.Organization.GetFilterOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,Company")]
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
        public async Task<GeneralResponse<PagedResult<GetAllOrganizationQueryResponse>>> OrganizationGetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            return await _mediator.Send(new GetAllOrganizationQueryRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
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
        [Authorize(Roles = "Admin,Company")]
        [HttpPut("OrganizationUpdate")]
        public async Task<GeneralResponse<UpdateOrganizationCommandResponse>> OrganizationUpdate([FromForm] UpdateOrganizationCommandRequest request)
        {
            
            return await _mediator.Send(request);
        }
        [HttpGet("Filter")]
        public async Task<GeneralResponse<PagedResult<GetFilteredOrganizationsQueryResponse>>>GetFiltered([FromQuery] GetFilteredOrganizationsQueryRequest query)
        {
            return await _mediator.Send(query);
            
        }
     //   [Authorize(Roles = "Admin,Company")]
        [HttpPost("GetFeatured")]
        public async Task<GeneralResponse<List<GetFeaturedQueryResponse>>> GetFeatured( GetFeaturedQueryRequest query)
        {
            return await _mediator.Send(query);
        }

     [HttpDelete("DeleteOrganization")]
     public async Task<GeneralResponse<DeleteOrganizationCommandResponse>> DeleteOrganization([FromForm] DeleteOrganizationCommandRequest query)
     {
         return await _mediator.Send(query);
     }


    }
}
