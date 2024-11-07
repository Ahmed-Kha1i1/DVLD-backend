namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery
{
    public class GetDetainedLicenseQuery : IRequest<Response<GetDetainedLicenseQueryResponse>>
    {
        public int DetainId { get; set; }

        public GetDetainedLicenseQuery(int detainedLicenseId)
        {
            DetainId = detainedLicenseId;
        }
    }
}
