using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.DeleteCompany;
using HappyDay.Application.Features.Commands.Company.UpdateCompany;
using HappyDay.Application.Features.Queries.Auth.OrganizationLogin;
using HappyDay.Application.Features.Queries.Category.GetAllCategory;
using HappyDay.Application.Features.Queries.Company.GetAllCompany;
using HappyDay.Application.Features.Queries.Company.GetByIdCompany;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<GeneralResponse<CreateCompanyCommandResponse>> AddCompany([FromForm] CreateCompanyCommandRequest request)
        {
            return await _mediator.Send(request);
        }
        
        [Authorize(Roles = "Admin,Company")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCompanyCommandRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }
        [Authorize(Roles = "Admin,Company")]
        [HttpDelete("delete")]
        public async Task<GeneralResponse<DeleteCompanyCommandResponse>> DeleteCompany([FromForm] DeleteCompanyCommandRequest request)
        {
            return await _mediator.Send(request);
        }
        [HttpPost("login")]
        public async Task<GeneralResponse<CompanyLoginQueryResponse>> LoginCompany( CompanyLoginQueryRequest request)
        {
            return await _mediator.Send(request);
        }
        [Authorize(Roles = "Admin,Company")]
        [HttpGet("getbyid")]
        public async Task<GeneralResponse<GetByIdCompanyQueryResponse>> GetByIdCompany([FromQuery] GetByIdCompanyQueryRequest request)
        {
            return await _mediator.Send(request);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("CompanyGetAll")]
        public async Task<GeneralResponse<List<GetAllCompanyQueryResponse>>> CompanyGetAll()
        {
            return await _mediator.Send(new GetAllCompanyQueryRequest());
        }

    }
}
