using HappyDay.Application.Features.Invitations;
using HappyDay.Application.Interface.Repository;
using HappyDay.Persistance.Context;
using HappyDay.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;

public class CompanyInvitationRepository : GenericRepository<CompanyInvitation>, ICompanyInvitationRepository
{
    private readonly HappyDayContext _context;
    public CompanyInvitationRepository(HappyDayContext appContext, HappyDayContext context) : base(appContext)
    {
        _context = context;
    }

    public async Task<ValidateInvitationResponse> ValidateAsync(string token, CancellationToken ct)
    {
        var nowUtc = DateTime.UtcNow; // tek sefer al
        var hash   = InvitationToken.Hash(token);

        var inv = await _context.CompanyInvitations
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.TokenHash == hash, ct);

        if (inv is null)
            return new ValidateInvitationResponse(false, "Token bulunamadı.");

        if (inv.IsUsed)
            return new ValidateInvitationResponse(false, "Token daha önce kullanılmış.");

        // DOĞRU KARŞILAŞTIRMA: Süresi dolmuş => ExpiresAt < now
        if (inv.ExpiresAt.HasValue && inv.ExpiresAt.Value < nowUtc)
            return new ValidateInvitationResponse(false, "Token süresi dolmuş.");

        return new ValidateInvitationResponse(true, null);
    }

    public async Task ConsumeAsync(string token, Guid createdCompanyId, CancellationToken ct)
    {
        var nowUtc = DateTime.UtcNow;
        var hash   = InvitationToken.Hash(token);

        var inv = await _context.CompanyInvitations
                      .FirstOrDefaultAsync(i => i.TokenHash == hash, ct)
                  ?? throw new InvalidOperationException("Token geçersiz.");

        if (inv.IsUsed)
            throw new InvalidOperationException("Token daha önce kullanılmış.");

        // DOĞRU KARŞILAŞTIRMA
        if (inv.ExpiresAt.HasValue && inv.ExpiresAt.Value < nowUtc)
            throw new InvalidOperationException("Token süresi dolmuş.");

        inv.UsedAt          = nowUtc;
        inv.UsedByCompanyId = createdCompanyId;

        await _context.SaveChangesAsync(ct);
    }
}