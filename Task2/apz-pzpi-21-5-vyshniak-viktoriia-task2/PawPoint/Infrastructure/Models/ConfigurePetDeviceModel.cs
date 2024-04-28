using BLL.Infrastructure.Models.Arduino;
using System;

namespace WebAPI.Infrastructure.Models;
public class ConfigurePetDeviceModel
{
    public Guid ArduinoSettingsId { get; set; }

    public WifiSettingsModel WifiSettings { get; set; }
}
