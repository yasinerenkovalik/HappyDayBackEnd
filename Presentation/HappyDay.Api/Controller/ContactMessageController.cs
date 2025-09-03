using HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        private readonly IMediator  _mediator;

        public ContactMessageController(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpPost("add")]
        public async Task<GeneralResponse<CreateContactMessageCommanResponse>> AddCompany([FromForm] CreateContactMessageCommanRequest request)
        {
            Console.WriteLine(request);
            return await _mediator.Send(request);
        }
    }
}
