using AutoMapper;
using BusinessLayer;
using DataLayerCore.Person;

namespace BusinessLayerCore.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            
            CreateMap<PersonDTO, clsPerson>().ReverseMap();
            CreateMap<clsPerson, PersonFullDTO>().ForMember(dest => dest.CountryName, src => src.MapFrom(src => src.CountryInfo != null ? src.CountryInfo.CountryName : ""));
        }
    }
}
