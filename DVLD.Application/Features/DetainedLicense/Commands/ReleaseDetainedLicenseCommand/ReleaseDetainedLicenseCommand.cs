namespace DVLD.Application.Features.DetainedLicense.Commands.ReleaseDetainedLicenseCommand
{
    public class ReleaseDetainedLicenseCommand : IRequest<Response<int?>>
    {
        public int LicenseId { get; set; }
        public int ReleasedByUserID { get; set; }
    }
}
