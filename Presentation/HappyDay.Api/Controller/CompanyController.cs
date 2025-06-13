using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.DeleteCompany;
using HappyDay.Application.Features.Queries.Auth.OrganizationLogin;
using HappyDay.Application.Features.Queries.Company.GetByIdCompany;
using HappyDay.Application.Wrappers;
using MediatR;
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
        [HttpPost("getbyid")]
        public async Task<GeneralResponse<GetByIdCompanyQueryResponse>> GetByIdCompany( GetByIdCompanyQueryRequest request)
        {
            return await _mediator.Send(request);
        }

    }
}
