using System.Text.Json.Serialization;

namespace Domain.Models;
public class RFIDSettings
{
    public int InstitutionId { get; set; }

    public string RFIDReaderIpAddress { get; set; }

    #region Relations

    [JsonIgnore]
    public Institution Institution { get; set; }

    #endregion
}
