using AutoMapper;
using BusinessLayer;
using DataLayerCore.User;

namespace BusinessLayerCore.Profiles
{
    public class userProfile : Profile
    {
        public userProfile()
        {

            CreateMap<UserDTO, clsUser>().ReverseMap();
            CreateMap<UserFordatabaseDTO, clsUser>().ReverseMap();
            CreateMap<clsUser, UserPrefDTO>();
            CreateMap<clsUser, UserFullDTO>().ForMember(dest => dest.Person,
                       opt => opt.MapFrom(src => src.PersonInfo));
            CreateMap<UserPrefDTO, UserFullDTO>();

        }
    }
}
