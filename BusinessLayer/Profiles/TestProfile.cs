using AutoMapper;
using BusinessLayer.Tests.Test;
using DataLayerCore.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<TestForCreateDTO, clsTest>().ReverseMap();
            CreateMap<TestForUpdateDTO, clsTest>().ReverseMap();
            CreateMap<TestDTO, clsTest >();
        }
    }
}
