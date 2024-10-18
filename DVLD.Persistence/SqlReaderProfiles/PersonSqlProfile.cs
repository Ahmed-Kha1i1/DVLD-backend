using AutoMapper;
using DVLD.Application.Features.People;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class PersonSqlProfile : Profile
    {
        public PersonSqlProfile()
        {
            CreateMap<IDataRecord, Person>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["PersonID"]))
                .ForMember(dis => dis.Gender, opt => opt.MapFrom(sc => Enum.ToObject(typeof(enGender), sc["Gender"])))
                .ForMember(dis => dis.DateOfBirth, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["DateOfBirth"])))
                .ForMember(dis => dis.CountryInfo, opt => opt.Ignore());

            CreateMap<IDataRecord, PersonOverviewDTO>()
                .ForMember(dis => dis.Gender, opt => opt.MapFrom(sc => Enum.ToObject(typeof(enGender), sc["Gender"])))
                .ForMember(dis => dis.DateOfBirth, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["DateOfBirth"]))); ;

        }
    }
}


