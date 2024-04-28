using AutoMapper;
using BLL.Contracts;
using BLL.Infrastructure.Models.Notification;
using DAL.UnitOfWork;
using Domain.Models;

namespace BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Lazy<IMapper> _mapper;

        public NotificationService(
            IUnitOfWork unitOfWork,
            Lazy<IMapper> mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void Create(int adminId, string petRFID)
        {
            var pet = _unitOfWork.Pets.Value.GetByRFID(petRFID);
            var notification = new Notification
            {
                PetId = pet.PetId,
                UserId = adminId,
            };

            _unitOfWork.Notifications.Value.Create(notification);
        }

        public IEnumerable<NotificationListItem> GetAllByUserId(int userId)
        {
            var notifications = _unitOfWork.Notifications.Value.GetAllByUserId(userId);
            var notificationModels = _mapper.Value.Map<List<NotificationListItem>>(notifications);

            return notificationModels;
        }

        public NotificationModel GetById(Guid notificationId)
        {
            var notification = _unitOfWork.Notifications.Value.GetById(notificationId);
            var notificationModel = _mapper.Value.Map<NotificationModel>(notification);

            return notificationModel;
        }
    }
}
