using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Common.Constants;
using Common.Enums;

namespace Domain.Models;
public class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.LOGIN_MIN_LENGTH, ErrorMessage = "INVALID_LOGIN_LENGTH")]
    [MaxLength(ValidationConstant.LOGIN_MAX_LENGTH, ErrorMessage = "INVALID_LOGIN_LENGTH")]
    [RegularExpression(RegularExpressions.LOGIN)]
    public string Login { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public string PasswordHash { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public string PasswordSalt { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime RegisteredOnUtc { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Role Role { get; set; } = Role.User;

    #region Relations

    [JsonIgnore]
    public UserProfile UserProfile { get; set; }

    [JsonIgnore]
    public ICollection<RefreshToken> RefreshTokens { get; set; }

    [JsonIgnore]
    public ICollection<ResetPasswordToken> ResetPasswordTokens { get; set; }

    [JsonIgnore]
    public ICollection<Pet> Pets { get; set; }

    [JsonIgnore]
    public ICollection<Institution> Institutions { get; set; }

    [JsonIgnore]
    public ICollection<Rating> Ratings { get; set; }

    [JsonIgnore]
    public ICollection<Notification> Notifications { get; set; }

    #endregion
}
