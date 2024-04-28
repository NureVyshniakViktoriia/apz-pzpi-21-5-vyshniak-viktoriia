using BLL.Infrastructure.Models.Notification;

namespace BLL.Contracts;
public interface INotificationService
{
    void Create(int adminId, string petRFID);

    IEnumerable<NotificationListItem> GetAllByUserId(int userId);

    NotificationModel GetById(Guid notificationId);
}
