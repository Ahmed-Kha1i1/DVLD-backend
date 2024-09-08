using AutoMapper;
using BusinessLayer.InternationalLicense;
using DataLayerCore.InternationalLicense;


namespace BusinessLayerCore.Profiles
{
    public class InternationalLicenseProfile : Profile
    {
        public InternationalLicenseProfile()
        {
            CreateMap<InternationalLicenseForCreateDTO, clsInternationalLicense>().ReverseMap();
            CreateMap<InternationalLicenseForUpdateDTO, clsInternationalLicense>().ReverseMap();
            CreateMap<InternationalLicenseDTO, clsInternationalLicense>();
        }
    }
}
