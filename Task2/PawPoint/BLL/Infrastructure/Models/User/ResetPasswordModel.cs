using System.ComponentModel.DataAnnotations;
using Common.Constants;

namespace BLL.Infrastructure.Models.User;
public class ResetPasswordModel
{
    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public string Token { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.PASSWORD_MIN_LENGTH)]
    [MaxLength(ValidationConstant.PASSWORD_MAX_LENGTH)]
    [RegularExpression(RegularExpressions.PASSWORD)]
    public string Password { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}
