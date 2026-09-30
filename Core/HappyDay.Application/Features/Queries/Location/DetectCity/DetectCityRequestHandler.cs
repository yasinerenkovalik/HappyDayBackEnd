using HappyDay.Application.Wrappers;
using HappyDay.Application.Interface.Repository;
using MediatR;

namespace HappyDay.Application.Features.Queries.Location.DetectCity;

public class DetectCityRequestHandler
    : IRequestHandler<DetectCityRequest, GeneralResponse<DetectCityResponse>>
{
    private readonly ICityesRepository _cities;
    private readonly IOrganizationRepository _organizations;

    public DetectCityRequestHandler(
        ICityesRepository cities,
        IOrganizationRepository organizations)
    {
        _cities = cities;
        _organizations = organizations;
    }

    public async Task<GeneralResponse<DetectCityResponse>> Handle(
        DetectCityRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Latitude is < -90 or > 90 || request.Longitude is < -180 or > 180)
        {
            return new GeneralResponse<DetectCityResponse>
            {
                isSuccess = false,
                Message = "Gecersiz koordinat."
            };
        }

        var allCities = await _cities.GetAllWithCoordinatesAsync();

        if (allCities.Count == 0)
        {
            return new GeneralResponse<DetectCityResponse>
            {
                isSuccess = false,
                Message = "Sehir verisi bulunamadi."
            };
        }

        // En yakin il merkezini bul (kucuk bir liste, dogrusal tarama yeterli)
        var nearest = allCities
            .Select(c => new
            {
                City = c,
                Distance = HaversineKm(request.Latitude, request.Longitude, c.Latitude, c.Longitude)
            })
            .OrderBy(x => x.Distance)
            .First();

        // Cok uzakta ise (deniz/yiurt disi ya da GPS hatasi) sonuc dondurme
        if (nearest.Distance > request.MaxDistanceKm)
        {
            return new GeneralResponse<DetectCityResponse>
            {
                isSuccess = false,
                Message = "Konumunuz Türkiye sinirlari disinda gorunuyor."
            };
        }

        // Bu ilde yayinda olan, sirketi onayli mekan sayisi
        var venueCount = await _organizations.CountPublishedByCityAsync(nearest.City.Id);

        return new GeneralResponse<DetectCityResponse>
        {
            Data = new DetectCityResponse
            {
                CityId = nearest.City.Id,
                CityName = nearest.City.CityName,
                DistanceKm = Math.Round(nearest.Distance, 1),
                VenueCount = venueCount
            },
            isSuccess = true,
            Message = "Konum belirlendi."
        };
    }

    /// <summary>İki nokta arası kuş uçuşu mesafe (km).</summary>
    private static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadiusKm = 6371.0;

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        return earthRadiusKm * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
}
