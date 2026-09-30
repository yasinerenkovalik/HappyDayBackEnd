using Microsoft.AspNetCore.Authorization;
using HappyDay.Application.Features.Commands.CalendarEvent.CreateCalendarEvent;
using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarEventsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Company")]
        public async Task<GeneralResponse<CreateCalenderEventCommandResponse>> AddCompany([FromForm] CreateCalenderEventCommandRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
