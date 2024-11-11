namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery
{
    public class GetDetainedLicensesByDateRangeQuery : IRequest<Response<IReadOnlyList<GetDetainedLicensesByDateRangeQueryResponse>>>
    {
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
