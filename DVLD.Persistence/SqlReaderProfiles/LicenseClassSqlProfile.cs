using AutoMapper;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class LicenseClassSqlProfile : Profile
    {
        public LicenseClassSqlProfile()
        {
            CreateMap<IDataRecord, LicenseClass>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src["LicenseClassID"]))
                .ForMember(des => des.ClassFees, opt => opt.MapFrom(src => src["ClassFees"]));
        }
    }
}
