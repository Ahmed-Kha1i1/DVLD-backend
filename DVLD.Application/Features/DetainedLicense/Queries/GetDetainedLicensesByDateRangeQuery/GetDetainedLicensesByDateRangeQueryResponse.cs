namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery
{
    public class GetDetainedLicensesByDateRangeQueryResponse
    {
        public int NumberofDetainedLicenes { get; set; }
        public DateOnly DetainDate { get; set; }
    }
}
