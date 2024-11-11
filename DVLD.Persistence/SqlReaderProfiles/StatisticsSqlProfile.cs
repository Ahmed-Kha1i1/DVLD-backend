using AutoMapper;
using DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery;
using System.Data;

namespace DVLD.Persistence.SqlReaderProfiles
{
    public class StatisticsSqlProfile : Profile
    {
        public StatisticsSqlProfile()
        {
            CreateMap<IDataRecord, GetAllStatisticsQueryResponse>()
                .ForMember(dis => dis.AllPaidFees, opt => opt.MapFrom(sc => sc["AllPaidFees"]));
        }
    }
}
