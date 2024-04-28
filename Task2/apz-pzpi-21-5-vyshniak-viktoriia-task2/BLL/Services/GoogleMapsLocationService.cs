using BLL.Contracts;
using Common.Configs;
using GoogleMaps.LocationServices;
using Microsoft.Extensions.Options;

namespace BLL.Services;
public class GoogleMapsLocationService : IGoogleMapsLocationService
{
    private readonly GoogleLocationService _googleLocationService;

    public GoogleMapsLocationService(GoogleMapsCreds googleMapsOptions)
    {
        _googleLocationService = new GoogleLocationService(googleMapsOptions.ApiKey);
    }

    public AddressData GetLocation(double latitude, double longitude)
    {
        var address = _googleLocationService.GetAddressFromLatLang(latitude, longitude);

        return address;
    }
}
