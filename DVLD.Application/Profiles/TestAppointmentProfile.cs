using AutoMapper;
using DVLD.Application.Features.TestAppointment.Queries.GetTestAppointmentQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class TestAppointmentProfile : Profile
    {
        public TestAppointmentProfile()
        {
            CreateMap<TestAppointment, GetTestAppointmentQueryResponse>()
                .ForMember(dis => dis.TestAppointmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dis => dis.RetakeTestApplication, opt => opt.Ignore());
        }
    }
}
