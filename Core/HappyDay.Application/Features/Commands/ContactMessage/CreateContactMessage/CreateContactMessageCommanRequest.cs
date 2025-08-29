using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage;

public class CreateContactMessageCommanRequest:IRequest<GeneralResponse<CreateContactMessageCommanResponse>>
{
    public string FullName { get; set; } 
    public string Phone { get; set; }
    public string Email { get; set; } 
    public string Message { get; set; }
    public Guid OrganizationId { get; set; }
  
}