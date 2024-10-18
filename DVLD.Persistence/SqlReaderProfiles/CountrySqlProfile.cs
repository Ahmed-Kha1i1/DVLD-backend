using AutoMapper;
using DVLD.Application.Features.Countries;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class CountrySqlProfile : Profile
    {
        public CountrySqlProfile()
        {
            CreateMap<IDataRecord, Country>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["CountryID"]));
            CreateMap<IDataRecord, CountryDTO>()
                .ForMember(dis => dis.CountryID, opt => opt.MapFrom(sc => sc["CountryID"])); ;
        }
    }
}


