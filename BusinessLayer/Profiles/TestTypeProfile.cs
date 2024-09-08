using AutoMapper;
using BusinessLayer.Tests.TestTypes;
using DataLayerCore.TestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class TesttypeProfile : Profile
    {
        public TesttypeProfile()
        {
            CreateMap<TestTypeForCreateDTO, clsTestType>().ReverseMap();
            CreateMap<TestTypeForUpdateDTO, clsTestType>().ReverseMap();
            CreateMap<TestTypeDTO, clsTestType>();
        }
    }
}
