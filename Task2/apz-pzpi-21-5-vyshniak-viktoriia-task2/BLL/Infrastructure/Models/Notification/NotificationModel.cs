using BLL.Infrastructure.Models.Pet;

namespace BLL.Infrastructure.Models.Notification;
public class NotificationModel
{
    public Guid NotificationId { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public PetNotificationProfile PetProfile { get; set; }
}
