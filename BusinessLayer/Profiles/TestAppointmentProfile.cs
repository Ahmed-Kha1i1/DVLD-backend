using AutoMapper;
using BusinessLayer.Tests.Test_Appointment;
using DataLayerCore.TestAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerCore.Profiles
{
    public class TestAppointmentProfile : Profile
    {
        public TestAppointmentProfile()
        {
            CreateMap<TestAppointmentForCreateDTO, clsTestAppointment>().ReverseMap();
            CreateMap<TestAppointmentForUpdateDTO, clsTestAppointment>().ReverseMap();
            CreateMap<TestAppointmentDTO, clsTestAppointment>();
        }
    }
}
