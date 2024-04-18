using AutoMapper;
using BLL.Infrastructure.Models.Facility;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class FacilityProfile : Profile
{
    public FacilityProfile()
    {
        CreateMap<Facility, FacilityModel>()
            .ReverseMap();

        CreateMap<CreateUpdateFacilityModel, Facility>();

        CreateMap<Facility, FacilityListItem>();
    }
}
