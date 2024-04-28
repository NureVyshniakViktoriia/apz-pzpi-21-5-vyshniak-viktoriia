using AutoMapper;
using BLL.Infrastructure.Models.Arduino;
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
           .ForMember(dest => dest.RFIDSettings,
                opt => opt.MapFrom(src => src.RFIDSettings))
           .ReverseMap();

        CreateMap<Institution, InstitutionListItem>();

        CreateMap<CreateUpdateInstitutionModel, Institution>();

        CreateMap<RFIDSettings, RFIDSettingsModel>();
    }
}
