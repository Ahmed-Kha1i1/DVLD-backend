namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicensesQuery
{
    public class GetInternationalLicensesQueryHandler(IInternationalLicenseRepository internationalLicenseRepository)
        : ResponseHandler, IRequestHandler<GetInternationalLicensesQuery, Response<GetInternationalLicensesQueryResposne>>
    {
        public async Task<Response<GetInternationalLicensesQueryResposne>> Handle(GetInternationalLicensesQuery request, CancellationToken cancellationToken)
        {
            var result = await internationalLicenseRepository.ListOverviewAsync(request);

            var resposne = new GetInternationalLicensesQueryResposne()
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
