using AutoMapper;
using DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseApplicationQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class InternationalLicenseProfile : Profile
    {
        public InternationalLicenseProfile()
        {
            CreateMap<InternationalLicense, GetInternationalLicenseApplicationQueryResponse>()
                .ForMember(dis => dis.internationalLicenseId, opt => opt.MapFrom(sc => sc.Id))
                .ForMember(dis => dis.ApplicationId, opt => opt.MapFrom(sc => sc.ApplicationID))
                .ForMember(dis => dis.ApplicationFees, opt => opt.MapFrom(sc => sc.ApplicationInfo.PaidFees))
                .ForMember(dis => dis.ApplicationDate, opt => opt.MapFrom(sc => sc.ApplicationInfo.ApplicationDate))
                .ForMember(dis => dis.LocalLicenseId, opt => opt.MapFrom(sc => sc.IssuedUsingLocalLicenseID))
                .ForMember(dis => dis.CreatedUserId, opt => opt.MapFrom(sc => sc.CreatedByUserID));
        }
    }
}
