using AutoMapper;
using BLL.Infrastructure.Models.User;
using Domain.Models;

namespace BLL.Infrastructure.Automapper;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserProfileModel>()
            .ForMember(dst => dst.Email, 
                opt => opt.MapFrom(src => src.UserProfile.Email))
            .ForMember(dst => dst.Gender,
                opt => opt.MapFrom(src => src.UserProfile.Gender))
            .ForMember(dst => dst.Region,
                opt => opt.MapFrom(src => src.UserProfile.Region))
            .ForMember(dst => dst.RegisteredOnUtc,
                opt => opt.MapFrom(src => src.RegisteredOnUtc.Date))
            .ReverseMap();

        CreateMap<User, UserModel>()
            .ReverseMap();
    }
}
