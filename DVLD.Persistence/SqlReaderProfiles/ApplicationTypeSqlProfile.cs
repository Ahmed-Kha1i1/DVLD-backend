using AutoMapper;
using DVLD.Application.Features.ApplicationType.Common.Models;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class ApplicationTypeSqlProfile : Profile
    {
        public ApplicationTypeSqlProfile()
        {
            CreateMap<IDataRecord, ApplicationType>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["ApplicationTypeId"]))
                .ForMember(dis => dis.ApplicationFees, opt => opt.MapFrom(sc => sc["ApplicationFees"]));

            CreateMap<IDataRecord, ApplicationTypeDTO>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["ApplicationTypeId"]))
                .ForMember(dis => dis.Fees, opt => opt.MapFrom(sc => sc["ApplicationFees"]))
                .ForMember(dis => dis.Title, opt => opt.MapFrom(sc => sc["ApplicationTypeTitle"]));
        }
    }
}
