using AutoMapper;
using BusinessLayer;
using DataLayerCore.Driver;
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
        }
    }
}
