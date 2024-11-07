using AutoMapper;
using DVLD.Application.Features.License.Common;
using DVLD.Application.Features.License.Common.Models;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class LicenseSqlProfile : Profile
    {
        public LicenseSqlProfile()
        {
            CreateMap<IDataRecord, LicenseDTO>()
                .ForMember(dis => dis.IssueReason, opt => opt.MapFrom(sc => Helpers.GetIssueReasonText((enIssueReason)Enum.ToObject(typeof(enIssueReason), sc["IssueReason"]))))
                .ForMember(dis => dis.DateOfBirth, opt => opt.MapFrom(sc => DateOnly.FromDateTime((DateTime)sc["DateOfBirth"])))
                .ForMember(dis => dis.IsDetained, opt => opt.MapFrom(sc => sc["isDetained"] != DBNull.Value));

            CreateMap<IDataRecord, License>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["LicenseId"]))
               .ForMember(dest => dest.IssueReason, opt => opt.MapFrom(src => (enIssueReason)Enum.ToObject(typeof(enIssueReason), src["IssueReason"])))
               .ForMember(dis => dis.LicenseClassID, opt => opt.MapFrom(sc => sc["LicenseClass"]))
               .ForMember(dest => dest.PaidFees, opt => opt.MapFrom(src =>
                   src["PaidFees"] != DBNull.Value ? (decimal)src["PaidFees"] : 0
               ));
        }

    }
}
