namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicensesQuery
{
    public class GetDetainedLicensesQueryHandler(IDetainedLicenseRepository detainedLicenseRepository)
        : ResponseHandler, IRequestHandler<GetDetainedLicensesQuery, Response<GetDetainedLicensesQueryResponse>>
    {
        public async Task<Response<GetDetainedLicensesQueryResponse>> Handle(GetDetainedLicensesQuery request, CancellationToken cancellationToken)
        {
            var result = await detainedLicenseRepository.ListOverviewAsync(request);

            var resposne = new GetDetainedLicensesQueryResponse()
            {
                Items = result.items,
                Metadata = new()
                {
                    TotalCount = result.totalCount,
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber
                }
            };
            return Success(resposne);
        }
    }
}
