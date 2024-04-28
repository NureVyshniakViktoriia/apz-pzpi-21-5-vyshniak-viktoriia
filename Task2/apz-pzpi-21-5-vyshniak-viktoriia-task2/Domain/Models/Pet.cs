using Common.Constants;
using Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Models;
public class Pet
{
    public Guid PetId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public int OwnerId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.NAME_MIN_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    [MaxLength(ValidationConstant.NAME_MAX_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    public string NickName { get; set;}

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public PetType PetType { get; set; } = PetType.Cat;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public DateTime BirthDate { get; set;} = DateTime.UtcNow;

    public string Breed { get; set;} = string.Empty;

    public double Weight { get; set;}

    public double Height { get; set;}

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Characteristics { get; set;} = string.Empty;

    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Illnesses { get; set; } = string.Empty;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Preferences { get; set; } = string.Empty;

    public string RFID { get; set; } = string.Empty;

    #region Relations

    [JsonIgnore]
    public User Owner { get; set; }

    [JsonIgnore]
    public ArduinoSettings ArduinoSettings { get; set; }

    [JsonIgnore]
    public ICollection<DiaryNote> DiaryNotes { get; set; }

    [JsonIgnore]
    public ICollection<HealthRecord> HealthRecords { get; set; }

    [JsonIgnore]
    public ICollection<Notification> Notifications { get; set; }

    #endregion
}
