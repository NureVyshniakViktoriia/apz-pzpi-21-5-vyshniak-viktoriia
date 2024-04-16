using Domain.Models;

namespace DAL.Contracts;
public interface INotificationRepository
{
    void Create(Notification notification);

    IQueryable<Notification> GetAllByUserId(int userId);

    Notification GetById(Guid notificationId);
}
