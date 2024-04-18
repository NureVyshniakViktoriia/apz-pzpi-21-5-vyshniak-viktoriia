using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.Facility;
public class CreateUpdateFacilityModel
{
    public int? FacilityId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.NAME_MIN_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    [MaxLength(ValidationConstant.NAME_MAX_LENGTH, ErrorMessage = "INVALID_NAME_LENGTH")]
    public string Name { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.DESCRIPTION_MIN_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    [MaxLength(ValidationConstant.DESCRIPTION_MAX_LENGTH, ErrorMessage = "INVALID_DESCRIPTION_LENGTH")]
    public string Description { get; set; }
}
