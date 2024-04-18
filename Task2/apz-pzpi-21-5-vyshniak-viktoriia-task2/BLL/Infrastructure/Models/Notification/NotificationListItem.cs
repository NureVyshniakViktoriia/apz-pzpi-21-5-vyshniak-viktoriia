namespace BLL.Infrastructure.Models.Notification;
public class NotificationListItem
{
    public Guid NotificationId { get; set; }

    public Guid? PetId { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
