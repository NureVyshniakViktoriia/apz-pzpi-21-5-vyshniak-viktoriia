using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace DAL.Infrastructure.Models;
public class UserProfileInfo
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    [EmailAddress(ErrorMessage = "INVALID_EMAIL")]
    public string Email { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public Region Region { get; set; }
}
