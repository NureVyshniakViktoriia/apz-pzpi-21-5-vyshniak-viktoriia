using Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class DiaryNote
{
    public Guid DiaryNoteId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Guid PetId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.NAME_MIN_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    [MaxLength(ValidationConstant.NAME_MAX_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    public string Title { get; set; }

    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Comment { get; set; }

    public byte[] FileBytes { get; set; } = new byte[0];

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime CreatedOnUtc { get; set; }

    public DateTime? LastUpdatedOnUtc { get; set; }

    #region Relations

    [JsonIgnore]
    public Pet Pet { get; set; }

    #endregion
}
