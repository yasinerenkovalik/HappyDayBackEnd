using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HappyDay.Application.Features.Commands.ContactMessage.CreateContactMessage;
using HappyDay.Application.Features.Queries.ContactMessage.GetByCompanyContactMessage;
using HappyDay.Application.Interface.Repository;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactMessageController : ControllerBase
    {
        private readonly IMediator  _mediator;

        public ContactMessageController(IMediator mediator)
        {
            _mediator = mediator;
            
        }

        [HttpPost("add")]
        public async Task<GeneralResponse<CreateContactMessageCommanResponse>> AddCompany([FromForm] CreateContactMessageCommanRequest request)
        {
           
            return await _mediator.Send(request);
        }

        // Sirket mesajlari yalnizca ilgili sirket yoneticisi tarafindan okunabilir.
        // CompanyId istemciden guvenilmez; JWT claim'i uzerinden belirlenir.
        [Authorize]
        [HttpPost("CompanyContactMessage")]
        public async Task<IActionResult> CompanyContactMessage([FromForm] GetByCompanyContactMessageRequest request)
        {
            var isAdmin = User.IsInRole("Admin");
            var claimCompanyId = User.FindFirstValue("CompanyId");

            if (!isAdmin)
            {
                if (string.IsNullOrWhiteSpace(claimCompanyId) || !Guid.TryParse(claimCompanyId, out _))
                {
                    return Unauthorized(new
                    {
                        isSuccess = false,
                        message = "Bu endpoint'e erisim icin firma yoneticisi yetkisi gerekir."
                    });
                }

                // Istemcinin gonderdigi CompanyId yok sayilir.
                request.CompanyId = Guid.Parse(claimCompanyId);
            }
            else if (request.CompanyId == Guid.Empty)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Admin icin CompanyId zorunludur."
                });
            }

            return Ok(await _mediator.Send(request));
        }
    }
}
