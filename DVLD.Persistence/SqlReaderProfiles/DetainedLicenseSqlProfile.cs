﻿using AutoMapper;
using DVLD.Application.Features.DetainedLicense.Common.Models;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class DetainedLicenseSqlProfile : Profile
    {
        public DetainedLicenseSqlProfile()
        {
            CreateMap<IDataRecord, DetainedLicenseOverviewDTO>()
                .ForMember(dis => dis.FineFees, opt => opt.MapFrom(sc => sc["FineFees"])); ;

            CreateMap<IDataRecord, DetainedLicense>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["DetainID"]))
                .ForMember(dis => dis.FineFees, opt => opt.MapFrom(sc => sc["FineFees"]));

            CreateMap<IDataRecord, GetDetainedLicensesByDateRangeQueryResponse>()
                .ForMember(dis => dis.NumberofDetainedLicenes, opt => opt.MapFrom(sc => sc["NumberofDetained"]))
                .ForMember(dis => dis.DetainDate, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["DetainDate"])));
        }
    }
}
