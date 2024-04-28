using AutoMapper;
using DAL.Infrastructure.Models;
using Domain.Models;

namespace DAL.Infrastructure.Automapper;
public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, Pet>();

        CreateMap<DiaryNote, DiaryNote>()
            .ForMember(dest => dest.CreatedOnUtc,
                opt => opt.Ignore()); ;

        CreateMap<DiaryNote, DocumentDataModel>();

        CreateMap<Notification, Notification>();
    }
}
