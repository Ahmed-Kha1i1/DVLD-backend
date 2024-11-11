using DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IStatisticsRepository
    {
        Task<GetAllStatisticsQueryResponse> GetAllStatistics(DateTime? StartDate, DateTime? EndDate);
    }
}
