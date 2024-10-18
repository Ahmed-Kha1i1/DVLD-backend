using AutoMapper;
using DVLD.Application.Features.Users;
using DVLD.Domain.Entities;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class UserSqlProfile : Profile
    {
        public UserSqlProfile()
        {
            CreateMap<IDataRecord, User>()
                .ForMember(dis => dis.Id, opt => opt.MapFrom(sc => sc["UserID"]))
                .ForMember(dis => dis.PersonInfo, opt => opt.Ignore());

            CreateMap<IDataRecord, UserOverviewDTO>();
        }
    }
}
