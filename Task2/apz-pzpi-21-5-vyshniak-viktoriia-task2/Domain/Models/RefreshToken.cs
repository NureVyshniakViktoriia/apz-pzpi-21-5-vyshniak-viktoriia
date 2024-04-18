using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class RefreshToken
{
    public Guid RefreshTokenId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime ExpiresOnUtc { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    #endregion
}
