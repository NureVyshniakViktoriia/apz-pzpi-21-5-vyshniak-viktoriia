using BLL.Infrastructure.Models.Notification;

namespace BLL.Contracts;
public interface INotificationService
{
    void Create(CreateNotificationModel notificationModel);

    IEnumerable<NotificationListItem> GetAllByUserId(int userId);

    NotificationModel GetById(Guid notificationId);
}
