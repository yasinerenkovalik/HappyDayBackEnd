using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Contact.CreateContact;

public class CreateContactCommandRequest: IRequest<GeneralResponse<CreateContactCommandResponse>>
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Mesaage { get; set; }
}