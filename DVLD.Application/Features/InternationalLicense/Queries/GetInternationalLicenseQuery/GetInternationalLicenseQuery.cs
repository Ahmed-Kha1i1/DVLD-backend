namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseQuery
{
    public class GetInternationalLicenseQuery : IRequest<Response<GetInternationalLicenseQueryResponse>>
    {
        public int InternationalLicenseId { get; set; }

        public GetInternationalLicenseQuery(int internationalLicenseId)
        {
            InternationalLicenseId = internationalLicenseId;
        }
    }
}
