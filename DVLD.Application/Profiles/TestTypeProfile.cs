using AutoMapper;
using DVLD.Application.Features.TestType.Commands.UpdateTestTypeCommand;
using DVLD.Application.Features.TestType.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Profiles
{
    public class TestTypeProfile : Profile
    {
        public TestTypeProfile()
        {
            CreateMap<UpdateTestTypeCommand, TestType>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.TestTypeId))
                .ForMember(des => des.TestTypeTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(des => des.TestTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(des => des.TestTypeFees, opt => opt.MapFrom(src => src.Fees));
            CreateMap<TestType, TestTypeDTO>()
                .ForMember(des => des.Title, opt => opt.MapFrom(src => src.TestTypeTitle))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.TestTypeDescription))
                .ForMember(des => des.Fees, opt => opt.MapFrom(src => src.TestTypeFees));
        }
    }
}
