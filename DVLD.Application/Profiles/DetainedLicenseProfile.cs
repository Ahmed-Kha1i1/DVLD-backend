using AutoMapper;
using DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class DetainedLicenseProfile : Profile
    {
        public DetainedLicenseProfile()
        {
            CreateMap<DetainedLicense, GetDetainedLicenseQueryResponse>()
                .ForMember(des => des.DetainedId, opt => opt.MapFrom(src => src.Id)); ;
        }
    }
}
