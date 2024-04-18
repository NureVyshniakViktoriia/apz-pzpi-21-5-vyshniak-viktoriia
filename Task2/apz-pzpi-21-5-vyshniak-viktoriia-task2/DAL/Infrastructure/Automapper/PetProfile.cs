using AutoMapper;
using DAL.Infrastructure.Models;
using Domain.Models;

namespace DAL.Infrastructure.Automapper;
public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, Pet>();

        CreateMap<DiaryNote, DiaryNote>();

        CreateMap<DiaryNote, DocumentDataModel>();

        CreateMap<Notification, Notification>();
    }
}
