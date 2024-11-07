namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery
{
    public class GetInternationalLicenseQueryHandler(IInternationalLicenseRepository internationalLicenseRepository) : ResponseHandler, IRequestHandler<GetInternationalLicenseQuery, Response<GetInternationalLicenseQueryResponse>>
    {
        public async Task<Response<GetInternationalLicenseQueryResponse>> Handle(GetInternationalLicenseQuery request, CancellationToken cancellationToken)
        {
            GetInternationalLicenseQueryResponse internationaLicense = await internationalLicenseRepository.GetInternationalLicense(request.InternationalLicenseId);
            if (internationaLicense is null)
            {
                return NotFound<GetInternationalLicenseQueryResponse>("International license not found");
            }

            return Success(internationaLicense);
        }
    }
}
