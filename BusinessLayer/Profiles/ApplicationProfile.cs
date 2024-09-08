using AutoMapper;
using BusinessLayer.ApplicationsDescendants.Applications;
using DataLayerCore.Application;

namespace DVLDApi.Profiles
{
        public class ApplicationProfile : Profile
        {
            public ApplicationProfile()
            {
                CreateMap<ApplicationForCreateDTO, clsApplication>().ReverseMap();
                CreateMap<ApplicationForUpdateDTO, clsApplication>().ReverseMap();
                CreateMap<clsApplication, ApplicationDTO>();
            }
        }
}
