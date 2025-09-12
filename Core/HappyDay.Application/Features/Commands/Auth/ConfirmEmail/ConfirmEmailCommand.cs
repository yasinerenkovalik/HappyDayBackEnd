// Core/HappyDay.Application/Features/Commands/Auth/ConfirmEmail/ConfirmEmailCommand.cs
using System;
using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Auth.ConfirmEmail
{
    public record ConfirmEmailCommand(Guid CompanyId, string Token) : IRequest<GeneralResponse<ConfirmEmailCommandResponse>>;

    public class ConfirmEmailCommandResponse
    {
        public bool Confirmed { get; set; }
        public string Message { get; set; } = "";
    }
}