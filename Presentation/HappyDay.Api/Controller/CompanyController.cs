using HappyDay.Application.Features.Commands.Company.CreateCompany;
using HappyDay.Application.Features.Commands.Company.DeleteCompany;
using HappyDay.Application.Features.Commands.Company.UpdateCompany;
using HappyDay.Application.Features.Commands.Auth.ConfirmEmail;
using HappyDay.Application.Features.Commands.Auth.ResendConfirmation;
using HappyDay.Application.Features.PasswordReset.RequestPasswordReset;
using HappyDay.Application.Features.PasswordReset.ResetPassword;
using HappyDay.Application.Features.Queries.Auth.OrganizationLogin;
using HappyDay.Application.Features.Queries.Company.GetAllCompany;
using HappyDay.Application.Features.Queries.Company.GetByIdCompany;
using HappyDay.Application.Wrappers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Şirket oluşturma (CreateCompany handler içinde e-posta onayı tetikleniyor)
        [HttpPost("add")]
        public async Task<GeneralResponse<CreateCompanyCommandResponse>> AddCompany([FromForm] CreateCompanyCommandRequest request)
        {
            return await _mediator.Send(request);
        }

        // Giriş (email onayı yoksa handler uygun mesaj dönecek)
        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<GeneralResponse<CompanyLoginQueryResponse>> LoginCompany([FromBody] CompanyLoginQueryRequest request)
        {
            return await _mediator.Send(request);
        }

        // E-posta onayı (maildeki linkten FRONTEND çağırır; body: { companyId, token })
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand request, CancellationToken ct)
        {
            var res = await _mediator.Send(request, ct);
            if (!res.isSuccess) return BadRequest(res);
            return Ok(res);
        }

        // Onay mailini tekrar gönder (UI'da "Onay maili gelmedi mi?" butonu)
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationCommand request, CancellationToken ct)
        {
            var res = await _mediator.Send(request, ct);
            // Bilgi sızdırmamak için her durumda 200 dönüyoruz (mesajdan anlaşılır)
            return Ok(res);
        }

       // [Authorize(Roles = "Admin,Company")]
        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateCompanyCommandRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [HttpDelete("delete")]
        public async Task<GeneralResponse<DeleteCompanyCommandResponse>> DeleteCompany([FromForm] DeleteCompanyCommandRequest request)
        {
            return await _mediator.Send(request);
        }

      
        [HttpGet("getbyid")]
        public async Task<GeneralResponse<GetByIdCompanyQueryResponse>> GetByIdCompany([FromQuery] GetByIdCompanyQueryRequest request)
        {
            return await _mediator.Send(request);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("CompanyGetAll")]
        public async Task<GeneralResponse<List<GetAllCompanyQueryResponse>>> CompanyGetAll()
        {
            return await _mediator.Send(new GetAllCompanyQueryRequest());
        }
        [AllowAnonymous]
        [HttpPost("request-password-reset")]
        public async Task<GeneralResponse<Unit>> RequestPasswordReset(
            [FromBody] RequestPasswordResetCommand req, CancellationToken ct)
            => await _mediator.Send(req, ct);

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<GeneralResponse<Unit>> ResetPassword(
            [FromBody] ResetPasswordCommand req, CancellationToken ct)
            => await _mediator.Send(req, ct);
    }
}
