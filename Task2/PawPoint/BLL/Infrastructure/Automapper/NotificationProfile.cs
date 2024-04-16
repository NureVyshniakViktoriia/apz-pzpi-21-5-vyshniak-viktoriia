using AutoMapper;
using BLL.Infrastructure.Models.Notification;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationListItem>();

        CreateMap<CreateNotificationModel, Notification>();

        CreateMap<Notification, NotificationModel>()
            .ForMember(dest => dest.PetProfile,
                opt => opt.MapFrom(src => src.Pet))
            .ForPath(dest => dest.PetProfile.Documents,
                opt => opt.MapFrom(src => src.Pet.DiaryNotes));

    }
}
