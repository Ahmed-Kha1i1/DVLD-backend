using AutoMapper;
using DVLD.Application.Features.TestAppointment.Common.Models;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class TestAppointmentSqlProfile : Profile
    {
        public TestAppointmentSqlProfile()
        {
            CreateMap<IDataRecord, TestAppointmentOverviewDTO>()
                .ForMember(des => des.PaidFees, opt => opt.MapFrom(src => src["PaidFees"]))
                .ForMember(dis => dis.AppointmentDate, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["AppointmentDate"])));

            CreateMap<IDataRecord, TestAppointment>()
               .ForMember(des => des.Id, opt => opt.MapFrom(src => src["TestAppointmentID"]))
               .ForMember(des => des.PaidFees, opt => opt.MapFrom(src => src["PaidFees"]))
               .ForMember(des => des.LocalApplicationID, opt => opt.MapFrom(src => src["LocalDrivingLicenseApplicationID"]))
               .ForMember(dis => dis.AppointmentDate, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["AppointmentDate"])));

            CreateMap<IDataRecord, RetakeTestApplicationDTO>()
                .ForMember(des => des.PaidFees, opt => opt.MapFrom(src => src["RetakePaidFees"]))
                .ForMember(des => des.RetakeApplicationId, opt => opt.MapFrom(src => src["RetakeApplicationId"]));
        }
    }
}
