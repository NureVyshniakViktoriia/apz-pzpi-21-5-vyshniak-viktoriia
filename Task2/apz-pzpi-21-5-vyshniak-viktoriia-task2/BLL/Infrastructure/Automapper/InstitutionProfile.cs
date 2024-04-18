using AutoMapper;
using BLL.Infrastructure.Models.Institution;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class InstitutionProfile : Profile
{
    public InstitutionProfile()
    {
        CreateMap<Institution, InstitutionModel>()
           .ForMember(dest => dest.Facilities,
                opt => opt.MapFrom(src => src.Facilities))
           .ReverseMap();

        CreateMap<Institution, InstitutionListItem>();

        CreateMap<CreateUpdateInstitutionModel, Institution>();
    }
}
