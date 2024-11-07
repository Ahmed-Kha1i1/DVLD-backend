
namespace DVLD.Application.Features.DetainedLicense.Commands.DetainLicenseCommand
{
    public class DetainLicenseCommandHandler(IDetainedLicenseRepository detainedLicenseRepository)
        : ResponseHandler, IRequestHandler<DetainLicenseCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(DetainLicenseCommand request, CancellationToken cancellationToken)
        {
            AllEntities.DetainedLicense detainedLicense = new();
            detainedLicense.LicenseID = request.LicenseId;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = request.FineFees;
            detainedLicense.CreatedByUserID = request.CreatedByUserID;

            if (!await detainedLicenseRepository.SaveAsync(detainedLicense))
            {
                return Fail<int?>(null, "Error detain license");
            }
            return Created<int?>(detainedLicense.Id);
        }
    }
}
