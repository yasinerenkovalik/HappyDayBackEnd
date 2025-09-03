using HappyDay.Application.Features.Invitations;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface ICompanyInvitationRepository:IGenericRepository<CompanyInvitation>
{
    Task<ValidateInvitationResponse> ValidateAsync(string token, CancellationToken ct);
    Task ConsumeAsync(string token, Guid createdCompanyId, CancellationToken ct);
}