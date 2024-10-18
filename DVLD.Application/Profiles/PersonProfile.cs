using AutoMapper;
using DVLD.Application.Features.People;
using DVLD.Application.Features.People.Commands.AddPersonCommand;
using DVLD.Application.Features.People.Commands.ModificationPerson;
using DVLD.Application.Features.People.Commands.UpdatePersonCommand;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonOverviewDTO>()
                .ForMember(dis => dis.FullName, opt => opt.MapFrom(sc => sc.FullName));

            CreateMap<Person, PersonDTO>()
                .ForMember(dis => dis.PersonID, opt => opt.MapFrom(sc => sc.Id))
                .ForMember(dis => dis.CountryName, opt => opt.MapFrom(sc => sc.CountryInfo.CountryName))
                .ForMember(dis => dis.Gender, opt => opt.MapFrom(sc => sc.Gender.ToString()));
            CreateMap<ModificationPersonCommand, Person>()
                .ForMember(dis => dis.FullName, opt => opt.Ignore())
                .ForMember(dis => dis.GenderCaption, opt => opt.Ignore())
                .ForMember(dis => dis.CountryInfo, opt => opt.Ignore())
                .ForMember(dis => dis.Gender, opt => opt.MapFrom(sc => Enum.Parse(typeof(enGender), sc.Gender)));
            CreateMap<AddPersonCommand, Person>();
            CreateMap<UpdatePersonCommand, Person>();
        }
    }
}
