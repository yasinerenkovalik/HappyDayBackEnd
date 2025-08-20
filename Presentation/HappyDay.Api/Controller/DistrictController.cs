using HappyDay.Application.Features.Queries.District.GetAllDistrict;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DistrictController(IMediator mediator)
        {
            _mediator = mediator;
        }      
        [HttpPost("GetAllDisctrictByCity")]
        public async Task<GeneralResponse<List<GetAllDisctrictByCityResponse>>> GetAllDisctrictByCity(GetAllDisctrictByCityRequest  request)
        {
            
            return await _mediator.Send(request);
        }
    }
}
