using AutoMapper;
using DVLD.Application.Features.ApplicationType.Commands.UpdateApplicationTypeCommand;
using DVLD.Application.Features.ApplicationType.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class ApplicationTypeProfile : Profile
    {
        public ApplicationTypeProfile()
        {
            CreateMap<UpdateApplicationTypeCommand, ApplicationType>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.ApplicationTypeId))
                .ForMember(des => des.ApplicationTypeTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.ApplicationFees, opt => opt.MapFrom(src => src.Fees));
            CreateMap<ApplicationType, ApplicationTypeDTO>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.ApplicationTypeTitle))
                .ForMember(des => des.Fees, opt => opt.MapFrom(src => src.ApplicationFees)); ;
        }
    }
}
