using System;

namespace HappyDay.Application.Features.Invitations
{
    public record CreateInvitationRequest(string? Email, string? CompanyNameHint, DateTime? ExpiresAt);
    public record CreateInvitationResponse(string Token, DateTime? ExpiresAt);
    public record ValidateInvitationResponse(bool IsValid, string? Reason);
}