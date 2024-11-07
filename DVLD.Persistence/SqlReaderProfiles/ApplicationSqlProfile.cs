using AutoMapper;
using DVLD.Application.Features.Application.Common.Model;
using DVLD.Domain.Common.Enums;
using System.Data;
using Entities = DVLD.Domain.Entities;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class ApplicationSqlProfile : Profile
    {
        public ApplicationSqlProfile()
        {
            CreateMap<IDataRecord, ApplicationOverviewDTO>()
                .ForMember(des => des.ApplicationStatusId, opt => opt.MapFrom(src => Enum.ToObject(typeof(enApplicationStatus), src["ApplicationStatus"])))
                .ForMember(des => des.PaidFees, opt => opt.MapFrom(src => src["PaidFees"]));

            CreateMap<IDataRecord, Entities.Application>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src["ApplicationID"]))
                .ForMember(des => des.ApplicationStatusID, opt => opt.MapFrom(src => Enum.ToObject(typeof(enApplicationStatus), src["ApplicationStatus"])))
                .ForMember(des => des.PaidFees, opt => opt.MapFrom(src => src["PaidFees"]));

        }
    }
}
