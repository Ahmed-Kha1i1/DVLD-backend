using AutoMapper;
using DVLD.Application.Features.Driver.Common.Model;
using DVLD.Application.Features.InternationalLicense.Common.Model;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class InternationalLicenseSqlProfile : Profile
    {
        public InternationalLicenseSqlProfile()
        {
            CreateMap<IDataRecord, DriverInternationalLicenseDTO>();
            CreateMap<IDataRecord, InternationalLicenseOverviewDTO>();
            CreateMap<IDataRecord, GetInternationalLicenseQueryResponse>()
                .ForMember(dis => dis.DateOfBirth, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["DateOfBirth"])));
            CreateMap<IDataRecord, InternationalLicense>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["InternationalLicenseID"]));

        }
    }
}
