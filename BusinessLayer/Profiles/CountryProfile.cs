using AutoMapper;
using BusinessLayer.Country;
using DataLayerCore.Country;

namespace DVLDApi.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<clsCountry, CountryDTO>();
        }
    }
}
