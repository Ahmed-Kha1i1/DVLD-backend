using ApiLayer.DTOs.Person;
using AutoMapper;
using BusinessLayer;

namespace ApiLayer.Profiles
{
    public class ModificationPersonProfile : Profile
    {
        public ModificationPersonProfile()
        {
            CreateMap<PersonForCreateDTO, clsPerson>();
            CreateMap<PersonForUpdateDTO, clsPerson>();
            CreateMap<ContactPersonDTO, clsPerson>();
        }
    }
}
