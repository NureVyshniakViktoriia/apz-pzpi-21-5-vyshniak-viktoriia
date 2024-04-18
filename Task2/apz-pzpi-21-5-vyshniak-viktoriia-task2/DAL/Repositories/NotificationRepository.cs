using DAL.Contracts;
using DAL.DbContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;
public class NotificationRepository : INotificationRepository
{
    private readonly DbContextBase _dbContext;
    private readonly DbSet<Notification> _notifications;

    public NotificationRepository(DbContextBase dbContext)
    {
        _dbContext = dbContext;
        _notifications = dbContext.Notifications;
    }

    public void Create(Notification notification)
    {
        notification.CreatedOnUtc = DateTime.UtcNow;
        _notifications.Add(notification);
        _dbContext.Commit();
    }

    public IQueryable<Notification> GetAllByUserId(int userId)
    {
        return _notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedOnUtc);
    }

    public Notification GetById(Guid notificationId)
    {
        var notification = _notifications
           .Include(n => n.Pet)
              .ThenInclude(p => p.DiaryNotes.Where(dn => dn.FileBytes != null))
           .FirstOrDefault(n => n.NotificationId == notificationId)
               ?? new Notification();

        return notification;
    }
}
