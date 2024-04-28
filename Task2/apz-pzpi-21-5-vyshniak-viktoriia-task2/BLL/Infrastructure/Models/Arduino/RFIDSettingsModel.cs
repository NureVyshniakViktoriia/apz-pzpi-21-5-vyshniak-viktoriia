using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.Arduino;
public class RFIDSettingsModel
{
    public int RFIDSettingsId { get; set; }

    public string RFIDReaderIpAddress { get; set; }

    [Required]
    public int InstitutionId { get; set; }
}