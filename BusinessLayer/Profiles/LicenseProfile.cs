using AutoMapper;
using BusinessLayer.License;
using DataLayerCore.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class LicenseProfile : Profile
    {
        public LicenseProfile()
        {
            CreateMap<LicenseInfoForCreateDTO, clsLicense>().ReverseMap();
            CreateMap<LicenseInfoForUpdateDTO, clsLicense>().ReverseMap();
            CreateMap<LicenseInfoDTO, clsLicense>();
        }
    }
}
