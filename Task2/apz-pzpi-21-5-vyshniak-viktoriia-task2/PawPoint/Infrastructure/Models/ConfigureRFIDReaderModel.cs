using BLL.Infrastructure.Models.Arduino;

namespace WebAPI.Infrastructure.Models;
public class ConfigureRFIDReaderModel
{
    public int RFIDSettingsId { get; set; }

    public WifiSettingsModel WifiSettings { get; set; }
}
