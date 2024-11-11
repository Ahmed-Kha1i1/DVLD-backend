namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesByDateRangeQuery
{
    public class GetDetainedLicensesByDateRangeQueryHnadler(IDetainedLicenseRepository detainedLicenseRepository)
        : ResponseHandler, IRequestHandler<GetDetainedLicensesByDateRangeQuery, Response<IReadOnlyList<GetDetainedLicensesByDateRangeQueryResponse>>>
    {
        public async Task<Response<IReadOnlyList<GetDetainedLicensesByDateRangeQueryResponse>>> Handle(GetDetainedLicensesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var items = await detainedLicenseRepository.GetDetainedLicensesByDateRange(request.StartDate, request.EndDate);
            return Success(items);
        }
    }
}
