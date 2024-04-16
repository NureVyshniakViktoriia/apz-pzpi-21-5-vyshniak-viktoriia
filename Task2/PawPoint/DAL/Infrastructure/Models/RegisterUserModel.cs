using System.ComponentModel.DataAnnotations;
using Common.Constants;
using Common.Enums;

namespace DAL.Infrastructure.Models;
public class RegisterUserModel
{
    public int UserId { get; set; }

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

    public Role Role { get; set; } = Role.User;

    public Region Region { get; set; }

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
    public string Email { get; set; }
}
