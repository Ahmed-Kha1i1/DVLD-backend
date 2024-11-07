using AutoMapper;
using DVLD.Application.Features.TestType.Common.Models;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class TestTypeSqlProfile : Profile
    {
        public TestTypeSqlProfile()
        {
            CreateMap<IDataRecord, TestType>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["TestTypeId"]))
            .ForMember(dis => dis.TestTypeFees, opt => opt.MapFrom(sc => sc["TestTypeFees"]));
            CreateMap<IDataRecord, TestTypeDTO>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["TestTypeId"]))
                .ForMember(dis => dis.Title, opt => opt.MapFrom(sc => sc["TestTypeTitle"]))
                .ForMember(dis => dis.Description, opt => opt.MapFrom(sc => sc["TestTypeDescription"]))
                .ForMember(dis => dis.Fees, opt => opt.MapFrom(sc => sc["TestTypeFees"]));
        }
    }
}
