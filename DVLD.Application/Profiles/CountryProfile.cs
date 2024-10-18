using AutoMapper;
using DVLD.Application.Features.Countries;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryDTO>()
                .ForMember(dis => dis.CountryID, opt => opt.MapFrom(sc => sc.Id));
        }
    }
}
