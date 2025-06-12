using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.User.CreateUser;

public class CreateUserCommandRequest:IRequest<GeneralResponse<CreateUserCommandResponse>>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SurName { get; set; }
    public DateTime BirtDay { get; set; }
    public int IdentityNo { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public bool Sex { get; set; }
}