using AutoMapper;
using HappyDay.Application.Features.Commands.Organization.CreateOrganization;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Features.Queries.Organization.GetByIdOrganization;
using HappyDay.Application.Features.Queries.Organization.GetOrganizationWithImages;
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
        [HttpPost("AddCompany")]
        public async Task<GeneralResponse<CreateOrganizationCommandResponse>> AddCompany([FromForm] CreateOrganizationCommandRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpPost("CompanyGetById")]
        public async Task<GeneralResponse<GetByIdOrganizationQueryResponse>> CompanyGetById([FromForm] GetByIdOrganizationQueryRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpGet("CompanyGetAll")]
        public async Task<GeneralResponse<List<GetAllOrganizationQueryResponse>>> CompanyGetAll()
        {
            
            return await _mediator.Send(new GetAllOrganizationQueryRequest());
        }
        [HttpGet("GetOrganizationWithImages")]
        public async Task<GeneralResponse<GetOrganizationWithImagesResponse>> GetOrganizationWithImages(Guid Id)
        {
            
            return await _mediator.Send(new GetOrganizationWithImagesQueryRequest { Id = Id });
        }
    }
}
