namespace DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery
{
    public class GetAllStatisticsQuery : IRequest<Response<GetAllStatisticsQueryResponse>>
    {
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
