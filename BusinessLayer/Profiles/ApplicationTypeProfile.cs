using AutoMapper;
using BusinessLayer.ApplicationTypes;
using DataLayerCore.Application;

namespace DVLDApi.Profiles
{
    public class ApplicationTypeProfile :Profile
    {
        public ApplicationTypeProfile()
        {
            CreateMap<clsApplicationType, ApplicationDTO>();
        }
    }
}
