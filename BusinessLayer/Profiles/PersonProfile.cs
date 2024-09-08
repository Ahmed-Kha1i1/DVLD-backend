using AutoMapper;
using BusinessLayer;
using DataLayerCore.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonForCreateDTO, clsPerson>().ReverseMap();
            CreateMap<PersonForUpdateDTO, clsPerson>().ReverseMap();
            CreateMap<PersonDTO, clsPerson>();
        }
    }
}
