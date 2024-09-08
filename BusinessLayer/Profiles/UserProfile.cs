using AutoMapper;
using BusinessLayer;
using DataLayerCore.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class userProfile : Profile
    {
        public userProfile()
        {
            CreateMap<UserForCreateDTO, clsUser>().ReverseMap();
            CreateMap<UserForUpdateDTO, clsUser>().ReverseMap();
            CreateMap<UserDTO, clsUser>();
            
        }
    }
}
