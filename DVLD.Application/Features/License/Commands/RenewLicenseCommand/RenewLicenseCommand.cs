namespace DVLD.Application.Features.License.Commands.RenewLicenseCommand
{
    public class RenewLicenseCommand : IRequest<Response<int?>>
    {
        public int OldLicenseId { get; set; }
        public int CreatedUserId { get; set; }
        public string Notes { get; set; }
    }
}
