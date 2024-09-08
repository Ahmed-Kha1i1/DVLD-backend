using AutoMapper;
using BusinessLayer.LicenseClass;
using DataLayerCore.LicenseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class LicenseCassProfile : Profile
    {
        public LicenseCassProfile()
        {
            CreateMap<LicenseClassForCreateDTO, clsLicenseClass>().ReverseMap();
            CreateMap<LicenseClassForUpdateDTO, clsLicenseClass>().ReverseMap();
            CreateMap<LicenseClassDTO, clsLicenseClass>();
        }
    }
}
