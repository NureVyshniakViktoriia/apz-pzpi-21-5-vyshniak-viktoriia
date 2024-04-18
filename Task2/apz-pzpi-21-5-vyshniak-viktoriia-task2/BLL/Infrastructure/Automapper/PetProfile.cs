using AutoMapper;
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
            .ReverseMap();

        CreateMap<CreateUpdatePetModel, Pet>();

        CreateMap<Pet, PetListItem>()
             .ForMember(dest => dest.PetTypeName,
                opt => opt.MapFrom(src => Resources.Get(src.PetType.ToString().ToUpper())));

        CreateMap<Pet, PetNotificationProfile>()
            .ForMember(dest => dest.Documents,
                opt => opt.MapFrom(src => src.DiaryNotes));

        CreateMap<HealthRecord, HealthRecordModel>();
    }
}
