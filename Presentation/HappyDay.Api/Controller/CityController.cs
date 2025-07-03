using HappyDay.Application.Features.Queries.Citys.GetAllCity;
using HappyDay.Application.Features.Queries.Organization.GetAllOrganization;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("CityGetAll")]
        public async Task<GeneralResponse<List<GetAllCityQueryResponse>>> OrganizationGetAll()
        {
            
            return await _mediator.Send(new GetAllCityQueryRequest());
        }
    }
}
