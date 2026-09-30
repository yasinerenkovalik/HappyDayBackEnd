using HappyDay.Application.Features.Queries.Location.DetectCity;
using HappyDay.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HappyDay.Api.Controller;

/// <summary>
/// Konum tabanlı yardımcı endpoint'ler (herkese açık).
/// </summary>
[Route("api/location")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Tarayıcı GPS konumundan en yakın ili bulur.
    /// GET /api/location/detect-city?latitude=41.01&amp;longitude=28.97
    /// </summary>
    [HttpGet("detect-city")]
    public async Task<GeneralResponse<DetectCityResponse>> DetectCity(
        [FromQuery] DetectCityRequest request)
    {
        return await _mediator.Send(request);
    }
}
