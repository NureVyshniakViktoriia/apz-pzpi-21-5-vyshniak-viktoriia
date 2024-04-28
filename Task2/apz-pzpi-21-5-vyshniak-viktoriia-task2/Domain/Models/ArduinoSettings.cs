using System.Text.Json.Serialization;

namespace Domain.Models;
public class ArduinoSettings
{
    public Guid PetId { get; set; }

    public string PetDeviceIpAddress { get; set; }

    #region Relations

    [JsonIgnore]
    public Pet Pet { get; set; }

    #endregion
}
