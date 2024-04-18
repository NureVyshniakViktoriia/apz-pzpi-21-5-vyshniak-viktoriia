using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Infrastructure.Models;
public class LoginModel
{
    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.LOGIN_MIN_LENGTH, ErrorMessage = "INVALID_LOGIN_LENGTH")]
    [MaxLength(ValidationConstant.LOGIN_MAX_LENGTH, ErrorMessage = "INVALID_LOGIN_LENGTH")]
    [RegularExpression(RegularExpressions.LOGIN)]
    public string Login { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [MinLength(ValidationConstant.PASSWORD_MIN_LENGTH, ErrorMessage = "INVALID_PASSWORD_LENGTH")]
    [MaxLength(ValidationConstant.PASSWORD_MAX_LENGTH, ErrorMessage = "INVALID_PASSWORD_LENGTH")]
    [RegularExpression(RegularExpressions.PASSWORD)]
    public string Password { get; set; }
}
