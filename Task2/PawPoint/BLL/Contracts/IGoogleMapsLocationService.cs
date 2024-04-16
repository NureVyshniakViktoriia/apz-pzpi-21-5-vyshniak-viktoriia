using GoogleMaps.LocationServices;

namespace BLL.Contracts;
public interface IGoogleMapsLocationService
{
    AddressData GetLocation(double latitude, double longitude);
}
