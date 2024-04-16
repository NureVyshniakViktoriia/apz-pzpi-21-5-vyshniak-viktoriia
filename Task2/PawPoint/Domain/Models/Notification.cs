using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class Notification
{
    public Guid NotificationId { get; set; }

    public Guid? PetId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime CreatedOnUtc { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Pet Pet { get; set; }

    #endregion
}
