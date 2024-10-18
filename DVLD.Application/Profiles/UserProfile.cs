using AutoMapper;
using DVLD.Application.Features.Users;
using DVLD.Application.Features.Users.Commands.AddUserCommand;
using DVLD.Application.Features.Users.Commands.UpdateUserCommand;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dis => dis.UserId, opt => opt.MapFrom(sc => sc.Id))
                .ForMember(dis => dis.FullName, opt => opt.MapFrom(sc => sc.PersonInfo != null ? sc.PersonInfo.FullName : null))
                .ForMember(dis => dis.Person, opt => opt.MapFrom(sc => sc.PersonInfo));

            CreateMap<User, UserOverviewDTO>()
               .ForMember(dis => dis.UserId, opt => opt.MapFrom(sc => sc.Id))
               .ForMember(dis => dis.FullName, opt => opt.MapFrom(sc => sc.PersonInfo != null ? sc.PersonInfo.FullName : null));
            CreateMap<AddUserCommand, User>();
            CreateMap<UpdateUserCommand, User>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc.UserId));
        }
    }
}
