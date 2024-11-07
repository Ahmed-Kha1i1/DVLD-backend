namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseApplicationQuery
{
    public class GetInternationalLicenseApplicationQuery : IRequest<Response<GetInternationalLicenseApplicationQueryResponse>>
    {
        public int InternationalLicenseId { get; set; }

        public GetInternationalLicenseApplicationQuery(int internationalLicenseId)
        {
            InternationalLicenseId = internationalLicenseId;
        }
    }
}
