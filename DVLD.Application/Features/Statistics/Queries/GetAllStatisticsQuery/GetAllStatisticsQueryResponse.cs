namespace DVLD.Application.Features.Statistics.Queries.GetAllStatisticsQuery
{
    public class GetAllStatisticsQueryResponse
    {
        public int NumberOfDetainedLicenses { get; set; }
        public int NumberOfApplications { get; set; }
        public decimal AllPaidFees { get; set; }
        public int NumberOfLicenses { get; set; }
        public int NumberOfCompletedApplications { get; set; }
        public int NumberOfCancelledApplications { get; set; }
        public int NumberOfNewApplications { get; set; }
    }
}
