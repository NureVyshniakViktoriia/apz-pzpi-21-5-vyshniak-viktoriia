using BLL.Contracts;
using GoogleMaps.LocationServices;

namespace BLL.Services;
public class GoogleMapsLocationService : IGoogleMapsLocationService
{
    private readonly GoogleLocationService _googleLocationService;

    public GoogleMapsLocationService(string apiKey)
    {
        _googleLocationService = new GoogleLocationService(apiKey);
    }

    public AddressData GetLocation(double latitude, double longitude)
    {
        var address = _googleLocationService.GetAddressFromLatLang(latitude, longitude);

        return address;
    }
}
