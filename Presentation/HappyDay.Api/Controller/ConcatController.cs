using HappyDay.Application.Features.Commands.Contact.CreateContact;
using HappyDay.Application.Features.Queries.Company.GetAllCompany;
using HappyDay.Application.Features.Queries.Contact.GetAllContact;
using HappyDay.Application.Features.Queries.Contact.GetByIdContact;
using HappyDay.Application.Features.Queries.User.GetByIdUser;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConcatController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ConcatController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("add")]
        public async Task<GeneralResponse<CreateContactCommandResponse>> AddContact([FromForm] CreateContactCommandRequest request)
        {
           
            return await _mediator.Send(request);
        }
        [HttpGet("ContactGetAll")]
        public async Task<GeneralResponse<List<GetAllContactQueryResponse>>> ContactGetAll()
        {
            return await _mediator.Send(new GetAllContactQueryRequest());
        }
        [HttpPost("getbyid")]
        public async Task<GeneralResponse<GetByIdContactQueryResponse>>  Getbyid(GetByIdContactQueryRequest request)
        {
            return await _mediator.Send(request);
            
        }
    }
}
