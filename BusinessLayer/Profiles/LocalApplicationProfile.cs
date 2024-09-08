using AutoMapper;
using BusinessLayer.NewLocalLicesnse;
using DataLayerCore.LocalDrivingLicenseApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class LocalApplicationProfile : Profile
    {
        public LocalApplicationProfile()
        {
            CreateMap<LocalDrivingLicenseApplicationForCreateDTO, clsLocalDrivingLicenseApplication>().ReverseMap();
            CreateMap<LocalDrivingLicenseApplicationForUpdateDTO, clsLocalDrivingLicenseApplication>().ReverseMap();
            CreateMap<LocalDrivingLicenseApplicationDTO, clsLocalDrivingLicenseApplication>();
        }
    }
}
