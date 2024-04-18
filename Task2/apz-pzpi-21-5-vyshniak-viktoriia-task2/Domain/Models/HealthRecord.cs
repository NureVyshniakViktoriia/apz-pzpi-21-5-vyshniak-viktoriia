using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class HealthRecord
{
    public Guid HealthRecordId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")] 
    public Guid PetId { get; set;}

    public double Temperature { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime CreatedOnUtc { get; set; }

    #region Relations

    [JsonIgnore]
    public Pet Pet { get; set; }

    #endregion
}
