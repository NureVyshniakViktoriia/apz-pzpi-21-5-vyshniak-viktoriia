using System.ComponentModel.DataAnnotations;

namespace WebAPI.Infrastructure.Models;
public class SetRatingModel
{
    [Required]
    public int InstitutionId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int Mark { get; set; }
}
