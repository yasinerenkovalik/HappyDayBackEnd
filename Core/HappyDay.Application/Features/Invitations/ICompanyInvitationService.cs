using System;
using System.Threading;
using System.Threading.Tasks;

namespace HappyDay.Application.Features.Invitations
{
    public interface ICompanyInvitationService
    {
        Task<CreateInvitationResponse> CreateAsync(Guid adminId, CreateInvitationRequest req, CancellationToken ct);
        Task<ValidateInvitationResponse> ValidateAsync(string token, CancellationToken ct);
        Task ConsumeAsync(string token, Guid createdCompanyId, CancellationToken ct);
    }
}