using System;
using System.Threading;
using System.Threading.Tasks;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
// InvitationToken Generate/Hash için; sende farklı namespace ise ona göre düzelt
using HappyDay.Persistance.Security;

namespace HappyDay.Application.Features.Invitations
{
    public class CompanyInvitationService : ICompanyInvitationService
    {
        private readonly ICompanyInvitationRepository _companyInvitationRepository;

        public CompanyInvitationService(ICompanyInvitationRepository companyInvitationRepository)
        {
            _companyInvitationRepository = companyInvitationRepository;
        }

        public async Task<CreateInvitationResponse> CreateAsync(
            Guid adminId,
            CreateInvitationRequest req,
            CancellationToken ct)
        {
            // 1) Plain token üret
            var token = InvitationToken.Generate();
            // 2) Hash’ini al (DB’ye hash yazacağız)
            var hash  = InvitationToken.Hash(token);

            // 3) Entity hazırla
            var entity = new CompanyInvitation
            {
                TokenHash        = hash,
                Email            = req.Email,
                CompanyNameHint  = req.CompanyNameHint,
                ExpiresAt        = req.ExpiresAt,
                CreateDate        = DateTime.UtcNow,
                
              
            };
            
            await _companyInvitationRepository.AddAsync(entity);
          
            
            return new CreateInvitationResponse(token, req.ExpiresAt);
        }

        public Task<ValidateInvitationResponse> ValidateAsync(string token, CancellationToken ct)
        {
         
            return _companyInvitationRepository.ValidateAsync(token, ct);
        }

        public Task ConsumeAsync(string token, Guid createdCompanyId, CancellationToken ct)
        {
            
            return _companyInvitationRepository.ConsumeAsync(token, createdCompanyId, ct);
        }
    }
}
