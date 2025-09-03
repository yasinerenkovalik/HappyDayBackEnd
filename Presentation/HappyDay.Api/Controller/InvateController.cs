using System;

using System.Security.Claims;

using HappyDay.Application.Features.Invitations;
using HappyDay.Application.Interface.Repository;
using HappyDay.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controllers
{
    // ===========================
    // 1) ADMIN: Davetiye Oluştur
    // ===========================
    [ApiController]
    [Route("api/admin/invitations")]
   
    public class AdminInvitationsController : ControllerBase
    {
        private readonly ICompanyInvitationService _invitationService;

        public AdminInvitationsController(ICompanyInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateInvitationRequest req, CancellationToken ct)
        {
            // JWT içinden admin id
            var adminIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                             User.FindFirstValue("nameid");
            if (string.IsNullOrWhiteSpace(adminIdStr) || !Guid.TryParse(adminIdStr, out var adminId))
                return Unauthorized(new { message = "Admin kimliği alınamadı." });

            var result = await _invitationService.CreateAsync(adminId, req, ct);
          
            return Ok(result);
        }
    }

 
    [ApiController]
    [Route("api/company")]
    public class CompanyAuthController : ControllerBase
    {
        private readonly ICompanyInvitationService _invitationService;
        private readonly ICompanyRepository _companyRepository;          // projendeki firma repository’n
                          

        public CompanyAuthController(
            ICompanyInvitationService invitationService,
            ICompanyRepository companyRepository
            )
        {
            _invitationService = invitationService;
            _companyRepository = companyRepository;
         
        }

        public class CompanyRegisterRequest
        {
            public string Token { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string CompanyName { get; set; } = default!;
            public string Password { get; set; } = default!;
            
            public string Adress { get; set; }
            public string PhoneNumber { get; set; }
            public string Description { get; set; }
         
        }

        [AllowAnonymous]
        [HttpPost("register-by-invite")]
        public async Task<IActionResult> RegisterByInvite([FromBody] CompanyRegisterRequest req, CancellationToken ct)
        {
         
            var v = await _invitationService.ValidateAsync(req.Token, ct);
            if (!v.IsValid)
                return BadRequest(new { message = v.Reason ?? "Token geçersiz." });

            try
            {
                // 2) Company oluştur (örnek alanlar)
                var company = new Company
                {
                    Id = Guid.NewGuid(),
                    Email = req.Email,
                    Name  = req.CompanyName,
                    Adress= req.Adress,
                    PhoneNumber = req.PhoneNumber,
                    Description = req.Description,
                    PasswordHash = req.Password,
                    CreateDate    = DateTime.UtcNow
                };

                await _companyRepository.AddAsync(company);

                await _invitationService.ConsumeAsync(req.Token, company.Id, ct);
                return Ok(new { message = "Kayıt başarılı.", companyId = company.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
