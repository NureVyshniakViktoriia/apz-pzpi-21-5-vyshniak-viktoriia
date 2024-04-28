using Newtonsoft.Json;

namespace BLL.Infrastructure.Models.Arduino;
public class TemperatureMonitorResponse : ArduinoResponseBase
{
    [JsonProperty("Payload")]
    public double Temperature { get; set; }
}
