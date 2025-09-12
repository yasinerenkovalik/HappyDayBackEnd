// Core/HappyDay.Application/Features/Commands/Auth/ResendConfirmation/ResendConfirmationCommand.cs
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Auth.ResendConfirmation
{
    public record ResendConfirmationCommand(string Email) : IRequest<GeneralResponse<ResendConfirmationCommandResponse>>;

    public class ResendConfirmationCommandResponse
    {
        public bool Sent { get; set; }
        public string Message { get; set; } = "";
    }
}