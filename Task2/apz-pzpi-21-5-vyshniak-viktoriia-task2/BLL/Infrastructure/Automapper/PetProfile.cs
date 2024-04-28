using AutoMapper;
using BLL.Infrastructure.Models.Arduino;
using BLL.Infrastructure.Models.HealthRecord;
using BLL.Infrastructure.Models.Pet;
using Common.Resources;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, PetModel>()
            .ForMember(dest => dest.HealthRecords,
                opt => opt.MapFrom(src => src.HealthRecords))
            .ForMember(dest => dest.DiaryNotes,
                opt => opt.MapFrom(src => src.DiaryNotes))
            .ForMember(dest => dest.ArduinoSettings,
                opt => opt.MapFrom(src => src.ArduinoSettings))
            .ReverseMap();

        CreateMap<CreateUpdatePetModel, Pet>();

        CreateMap<Pet, PetListItem>()
             .ForMember(dest => dest.ArduinoSettings,
                opt => opt.MapFrom(src => src.ArduinoSettings))
             .ForMember(dest => dest.OwnerLogin,
                opt => opt.MapFrom(src => src.Owner.Login));

        CreateMap<Pet, PetNotificationProfile>()
            .ForMember(dest => dest.Documents,
                opt => opt.MapFrom(src => src.DiaryNotes));

        CreateMap<HealthRecord, HealthRecordModel>();

        CreateMap<ArduinoSettings, ArduinoSettingsModel>();
    }
}
