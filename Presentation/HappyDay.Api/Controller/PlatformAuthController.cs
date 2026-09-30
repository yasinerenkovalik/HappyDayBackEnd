using HappyDay.Application.Features.Queries.Auth.PlatformLogin;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller;

/// <summary>
/// Platform genel giriş noktası. Kullanıcı (admin), firma ve normal kullanıcı hesaplarını kabul eder.
/// </summary>
[Route("api/auth")]
[ApiController]
public class PlatformAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlatformAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<GeneralResponse<PlatformLoginQueryResponse>> Login(
        [FromForm] PlatformLoginQueryRequest request)
    {
        return await _mediator.Send(request);
    }
}
