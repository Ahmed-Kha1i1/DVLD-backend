namespace DVLD.Application.Features.DetainedLicense.Commands.DetainLicenseCommand
{
    public class DetainLicenseCommand : IRequest<Response<int?>>
    {
        public int LicenseId { get; set; }
        public int CreatedByUserID { get; set; }
        public float FineFees { get; set; }
    }
}
