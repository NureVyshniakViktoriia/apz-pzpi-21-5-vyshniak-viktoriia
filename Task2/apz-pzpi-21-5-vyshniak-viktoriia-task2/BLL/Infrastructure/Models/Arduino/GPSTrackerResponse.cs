using GoogleMaps.LocationServices;
using Newtonsoft.Json;

namespace BLL.Infrastructure.Models.Arduino;
public class GPSTrackerResponse : ArduinoResponseBase
{
    [JsonProperty("Latitutde")]
    public double Latitutde { get; set; }

    [JsonProperty("Longtitude")]
    public double Longtitude { get; set; }

    public AddressData AddressData { get; set; }
}
