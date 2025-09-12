using MediatR;
using HappyDay.Application.Wrappers;

namespace HappyDay.Application.Features.Invitations.RegisterByInvite;

public class RegisterByInviteCommand : IRequest<GeneralResponse<RegisterByInviteResponse>>
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string? Adress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Description { get; set; }
}

public class RegisterByInviteResponse
{
    public Guid CompanyId { get; set; }
    public bool EmailConfirmationRequired { get; set; } = true;
    public string? Info { get; set; }
}