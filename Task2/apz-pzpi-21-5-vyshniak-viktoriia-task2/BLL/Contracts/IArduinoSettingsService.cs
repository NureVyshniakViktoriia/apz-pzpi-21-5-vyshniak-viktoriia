using BLL.Infrastructure.Models.Arduino;
using GoogleMaps.LocationServices;

namespace BLL.Contracts;
public interface IArduinoSettingsService
{
    ConfigureResponse ConfigurePetDevice(WifiSettingsModel wifiSettings, Guid arduinoSettingsId);

    ConfigureResponse ConfigureRFIDReader(WifiSettingsModel wifiSettings, int rfidSettingsId);

    Task<TemperatureMonitorResponse> GetPetCurrentTemperature(Guid petId);

    Task<TemperatureMonitorResponse> GetPetAverageTemperature(Guid petId);

    Task<GPSTrackerResponse> GetPetCurrentLocation(Guid petId);
}
