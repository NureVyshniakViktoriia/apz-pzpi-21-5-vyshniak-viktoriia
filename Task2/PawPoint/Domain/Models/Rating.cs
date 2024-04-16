using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class Rating
{
    public Guid RatingId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public byte Mark { get; set; }

    public int? UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public int InstitutionId { get; set; }

    #region Relations

    [JsonIgnore]
    public User User { get; set; }

    [JsonIgnore]
    public Institution Institution { get; set; }

    #endregion
}
