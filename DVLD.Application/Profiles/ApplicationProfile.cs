using AutoMapper;
using DVLD.Application.Features.Application.Queries.GetApplicationQuery;

namespace DVLD.Application.Profiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<AllEntities.Application, GetApplicationQueryResponse>()
                .ForMember(des => des.ApplicationId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
