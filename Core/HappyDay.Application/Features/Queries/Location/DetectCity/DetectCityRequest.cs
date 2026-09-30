using HappyDay.Application.Wrappers;
using MediatR;

namespace HappyDay.Application.Features.Queries.Location.DetectCity;

/// <summary>Tarayıcı GPS konumundan en yakın ili bulur.</summary>
public class DetectCityRequest : IRequest<GeneralResponse<DetectCityResponse>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    /// <summary>Bu mesafeden (km) yakındaysa il olarak kabul edilir.</summary>
    public double MaxDistanceKm { get; set; } = 120;
}

public class DetectCityResponse
{
    public int CityId { get; set; }
    public string CityName { get; set; } = default!;
    public double DistanceKm { get; set; }

    /// <summary>Bu ilde yayında olan kaç mekan var (boş sonuçta yönlendirme için).</summary>
    public int VenueCount { get; set; }
}
