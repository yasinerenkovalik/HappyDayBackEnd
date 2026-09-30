using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Commands.Admin.SetUserActive;

public class SetUserActiveCommand : IRequest<GeneralResponse<SetUserActiveResponse>>
{
    public Guid UserId { get; set; }
    public bool IsActivated { get; set; }
}

public class SetUserActiveResponse
{
    public Guid UserId { get; set; }
    public bool IsActivated { get; set; }
}
