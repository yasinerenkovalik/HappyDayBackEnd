using System;

using System.Security.Claims;

using HappyDay.Application.Features.Invitations;
using HappyDay.Application.Features.Invitations.RegisterByInvite;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using HappyDay.Domain.Entities;
using MediatR;
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
        private readonly IMediator _mediator;


        public CompanyAuthController(
            ICompanyInvitationService invitationService,
            ICompanyRepository companyRepository,
            IMediator  mediator
            )
        {
            _invitationService = invitationService;
            _companyRepository = companyRepository;
            _mediator = mediator;
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
        public async Task<GeneralResponse<RegisterByInviteResponse>> RegisterByInvite(
            [FromBody] RegisterByInviteCommand req, CancellationToken ct)
            => await _mediator.Send(req, ct);
       
    }
}
