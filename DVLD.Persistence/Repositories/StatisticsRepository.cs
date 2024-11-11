using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery;
using DVLD.Persistence.Handlers;
using System.Data;

namespace DVLD.Persistence.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly DataSendhandler _dataSendhandler;
        private readonly IMapper _mapper;
        public StatisticsRepository(DataSendhandler dataSendhandler, IMapper mapper)
        {
            _dataSendhandler = dataSendhandler;
            _mapper = mapper;
        }
        public async Task<GetAllStatisticsQueryResponse> GetAllStatistics(DateTime? StartDate, DateTime? EndDate)
        {

            GetAllStatisticsQueryResponse statistics = new();
            await _dataSendhandler.Handle("SP_GetStatistics", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@StartDate", StartDate);
                Command.Parameters.AddWithValue("@EndtDate", EndDate);


                Command.Parameters.Add("@NumberOfDetainedLicenses", SqlDbType.Int).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@NumberOfApplications", SqlDbType.Int).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@AllPaidFees", SqlDbType.Decimal).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@NumberOfLicenses", SqlDbType.Int).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@NumberOfCompletedApplications", SqlDbType.Int).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@NumberOfCancelledApplications", SqlDbType.Int).Direction = ParameterDirection.Output;
                Command.Parameters.Add("@NumberOfNewApplications", SqlDbType.Int).Direction = ParameterDirection.Output;


                Connection.Open();
                await Command.ExecuteNonQueryAsync();

                statistics.NumberOfDetainedLicenses = (int)Command.Parameters["@NumberOfDetainedLicenses"].Value;
                statistics.NumberOfApplications = (int)Command.Parameters["@NumberOfApplications"].Value;
                statistics.AllPaidFees = (decimal)Command.Parameters["@AllPaidFees"].Value;
                statistics.NumberOfLicenses = (int)Command.Parameters["@NumberOfLicenses"].Value;
                statistics.NumberOfCompletedApplications = (int)Command.Parameters["@NumberOfCompletedApplications"].Value;
                statistics.NumberOfCancelledApplications = (int)Command.Parameters["@NumberOfCancelledApplications"].Value;
                statistics.NumberOfNewApplications = (int)Command.Parameters["@NumberOfNewApplications"].Value;
            });


            return statistics;
        }
    }
}
