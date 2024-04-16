using System.ComponentModel.DataAnnotations;

namespace WebAPI.Infrastructure.Models;
public class ForgotPasswordModel
{
    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
    public string Email { get; set; }
}
