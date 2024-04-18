using Common.Constants;
using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.Pet;
public class CreateUpdatePetModel
{
    public Guid? PetId { get; set; }

    public int OwnerId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.NAME_MIN_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    [MaxLength(ValidationConstant.NAME_MAX_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    public string NickName { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public PetType PetType { get; set; }

    public DateTime BirthDate { get; set; }

    public string Breed { get; set; }

    public double Weight { get; set; }

    public double Height { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Characteristics { get; set; } = string.Empty;

    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Illnesses { get; set; } = string.Empty;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Preferences { get; set; } = string.Empty;

    public string RFID { get; set; } = string.Empty;
}
