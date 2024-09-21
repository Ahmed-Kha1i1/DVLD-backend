using AutoMapper;
using BusinessLayer;
using DataLayerCore.Driver;
using DataLayerCore.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<DriverForCreateDTO, clsDriver>().ReverseMap();
            CreateMap<DriverForUpdateDTO, clsDriver>().ReverseMap();
            CreateMap<DriverDTO, clsDriver>();
            CreateMap<clsDriver, DriverPrefDTO>();
            CreateMap<clsDriver, DriverFullDTO>().ForMember(dest => dest.Person,
                       opt => opt.MapFrom(src => src.PersonInfo));
            CreateMap<UserPrefDTO, UserFullDTO>();
        }
    }
}
