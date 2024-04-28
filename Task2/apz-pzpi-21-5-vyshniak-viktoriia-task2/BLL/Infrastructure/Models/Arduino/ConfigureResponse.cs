using Newtonsoft.Json;

namespace BLL.Infrastructure.Models.Arduino;
public class ConfigureResponse : ArduinoResponseBase
{
    [JsonProperty("Payload")]
    public string IpAddress { get; set; }
}
