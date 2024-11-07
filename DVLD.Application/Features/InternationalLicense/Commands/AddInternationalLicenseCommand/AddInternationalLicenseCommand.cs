namespace DVLD.Application.Features.InternationalLicense.Commands.AddInternationalLicenseCommand
{
    public class AddInternationalLicenseCommand : IRequest<Response<int?>>
    {
        public int LicenseId { get; set; }
        public int CreatedUserId { get; set; }
    }
}
