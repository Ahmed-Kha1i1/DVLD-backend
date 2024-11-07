using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.DetainedLicense.Commands.ReleaseDetainedLicenseCommand
{
    public class ReleaseDetainedLicenseCommandHandler
        (IDetainedLicenseRepository detainedLicenseRepository, ILicenseRepository licenseRepository,
         IApplicationTypeRepository applicationTypeRepository, IApplicationRepository applicationRepository)
        : ResponseHandler, IRequestHandler<ReleaseDetainedLicenseCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(ReleaseDetainedLicenseCommand request, CancellationToken cancellationToken)
        {
            AllEntities.DetainedLicense? detainedLicense = await detainedLicenseRepository.GetByLicenseIdAsync(request.LicenseId);
            if (detainedLicense is null)
            {
                return NotFound<int?>("Detained license not found");
            }

            Task<AllEntities.ApplicationType?> applicationTypeTask = applicationTypeRepository.GetByIdAsync((int)enApplicationType.ReleaseDetainedDrivingLicsense);
            Task<int?> personIdTask = licenseRepository.GetPersonId(request.LicenseId);
            await Task.WhenAll(applicationTypeTask, personIdTask);

            if (applicationTypeTask.Result == null || personIdTask.Result == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for releasing.");
            }


            AllEntities.Application Application = new();
            Application.ApplicantPersonID = personIdTask.Result ?? 0;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatusID = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = applicationTypeTask.Result.ApplicationFees;
            Application.CreatedByUserID = request.ReleasedByUserID;

            if (!await applicationRepository.SaveAsync(Application))
            {
                return Fail<int?>(null, "Error adding application");
            }

            if (!await detainedLicenseRepository.ReleaseDetainedLicense(detainedLicense.Id, request.ReleasedByUserID, Application.Id))
            {
                return Fail<int?>(null, "Error releasing detained license");
            }


            return Created<int?>(Application.Id);
        }
    }
}
