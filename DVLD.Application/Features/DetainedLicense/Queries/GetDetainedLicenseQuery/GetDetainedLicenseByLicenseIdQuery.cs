namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery
{
    public class GetDetainedLicenseByLicenseIdQuery : IRequest<Response<GetDetainedLicenseQueryResponse>>
    {
        public int LicenseId { get; set; }

        public GetDetainedLicenseByLicenseIdQuery(int licenseId)
        {
            LicenseId = licenseId;
        }
    }
}
