using Common.Constants;
using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.Institution;
public class CreateUpdateInstitutionModel
{
    public int? InstitutionId { get; set; }

    public int OwnerId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.NAME_MIN_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    [MaxLength(ValidationConstant.NAME_MAX_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    public string Name { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Description { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [RegularExpression(RegularExpressions.PHONE_NUMBER, ErrorMessage = "INVALID_PHONE_NUMBER")]
    public string PhoneNumber { get; set; }

    public string Address { get; set; } = string.Empty;

    public string WebsiteUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Region Region { get; set; } = Region.Kyiv;

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public InstitutionType InstitutionType { get; set; }
}
