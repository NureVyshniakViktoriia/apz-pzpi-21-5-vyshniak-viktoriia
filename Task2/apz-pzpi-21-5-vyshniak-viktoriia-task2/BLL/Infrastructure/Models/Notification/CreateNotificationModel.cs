using System.ComponentModel.DataAnnotations;

namespace BLL.Infrastructure.Models.Notification;
public class CreateNotificationModel
{
    public Guid NotificationId { get; set; }

    public Guid? PetId { get; set; }

    [Required(ErrorMessage = "REQUIRED_FIELD")]
    public int UserId { get; set; }
}
