using AutoMapper;
using DVLD.Application.Features.Test.Commands.AddTestCommand;
using DVLD.Application.Features.Test.Commands.GetTestQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class TestProfile : Profile
    {
        public TestProfile()
        {
            CreateMap<Test, GetTestQueryResponse>();
            CreateMap<AddTestCommand, Test>()
                .ForMember(dis => dis.TestAppointmentID, opt => opt.MapFrom(sc => sc.TestAppointmentId))
                .ForMember(dis => dis.TestResult, opt => opt.MapFrom(sc => sc.Result))
                .ForMember(dis => dis.CreatedByUserID, opt => opt.MapFrom(sc => sc.CreatedUserId));
        }
    }
}
