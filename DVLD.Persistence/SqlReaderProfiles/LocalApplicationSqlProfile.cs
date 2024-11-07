using AutoMapper;
using DVLD.Application.Features.LocalApplication.Common.Models;
using DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationPerTestTypeQuery;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class LocalApplicationSqlProfile : Profile
    {
        public LocalApplicationSqlProfile()
        {
            CreateMap<IDataRecord, LocalApplicationOverviewDTO>()
                .ForMember(dis => dis.PassedTestCount, opt => opt.MapFrom(sc => sc["PassedTestCount"]));

            CreateMap<IDataRecord, LocalApplicationPrefDTO>()
                .ForMember(dis => dis.PaidFees, opt => opt.MapFrom(sc => sc["PaidFees"]));

            CreateMap<IDataRecord, LocalApplicationDTO>()
            .ForMember(dis => dis.PassedTestCount, opt => opt.MapFrom(sc => sc["PassedTestCount"]))
            .ForMember(dis => dis.basicApplication, opt => opt.Ignore());

            CreateMap<IDataRecord, LocalApplication>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src["LocalDrivingLicenseApplicationID"]));
            CreateMap<IDataRecord, GetLocalApplicationPerTestTypeQueryResponse>();
        }
    }
}
