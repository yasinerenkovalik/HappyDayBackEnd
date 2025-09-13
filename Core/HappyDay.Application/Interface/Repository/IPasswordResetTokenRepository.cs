using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository
{
    public interface IPasswordResetTokenRepository
    {
        Task AddAsync(PasswordResetToken token, CancellationToken ct = default);
        Task<PasswordResetToken?> GetValidAsync(Guid companyId, string tokenHash, string purpose, CancellationToken ct = default);
        Task UpdateAsync(PasswordResetToken token, CancellationToken ct = default);
        // (Teşhis için) toplam sayım:
        Task<int> CountByCompanyAsync(Guid companyId, CancellationToken ct = default);

       
    }
}