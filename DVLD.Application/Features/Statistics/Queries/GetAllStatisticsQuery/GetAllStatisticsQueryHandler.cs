namespace DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery
{
    public class GetAllStatisticsQueryHandler(IStatisticsRepository statisticsRepository)
        : ResponseHandler, IRequestHandler<GetAllStatisticsQuery, Response<GetAllStatisticsQueryResponse>>
    {
        public async Task<Response<GetAllStatisticsQueryResponse>> Handle(GetAllStatisticsQuery request, CancellationToken cancellationToken)
        {
            var items = await statisticsRepository.GetAllStatistics(request.StartDate, request.EndDate);
            return Success(items);
        }
    }
}
