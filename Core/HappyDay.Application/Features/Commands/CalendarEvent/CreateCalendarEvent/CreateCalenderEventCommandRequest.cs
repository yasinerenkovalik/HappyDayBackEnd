using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.CalendarEvent.CreateCalendarEvent;

public class CreateCalenderEventCommandRequest:IRequest<GeneralResponse<CreateCalenderEventCommandResponse>>
{
     
    public string Title { get; set; } = null!;
        
    public string? Description { get; set; }
        
    public DateTime StartUtc { get; set; }
        
    public DateTime EndUtc { get; set; }
        
    public Guid CompanyId { get; set; }

    public bool IsPublic { get; set; } = true;

    public bool IsAllDay { get; set; } = false;
}