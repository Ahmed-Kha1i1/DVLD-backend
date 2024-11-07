using AutoMapper;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class DriverSqlProfile : Profile
    {
        public DriverSqlProfile()
        {
            CreateMap<IDataRecord, DriverOverviewDTO>()
                .ForMember(dis => dis.NumberofActiveLicenses, opt => opt.MapFrom(sc => sc["NumberofActiveLicenses"]));
            CreateMap<IDataRecord, Driver>();

            CreateMap<IDataRecord, DriverDTO>()
                .ForMember(dis => dis.NumberofActiveLicenses, opt => opt.MapFrom(sc => sc["NumberofActiveLicenses"]));
            CreateMap<IDataRecord, DriverLicenseDTO>();
            CreateMap<IDataRecord, DriverInternationalLicenseDTO>();
        }
    }
}
