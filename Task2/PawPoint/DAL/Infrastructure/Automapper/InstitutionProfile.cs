using AutoMapper;
using Domain.Models;

namespace DAL.Infrastructure.Automapper;
public class InstitutionProfile : Profile
{
    public InstitutionProfile()
    {
        CreateMap<Institution, Institution>();

        CreateMap<Facility, Facility>();
    }
}
