using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Enums;

namespace Domain.Models;
public class UserProfile
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Region Region { get; set; } = Region.Kyiv;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Gender Gender { get; set; } = Gender.Male;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
    public string Email { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    #endregion
}
