
using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.License.Commands.ReplaceLicenseCommand
{
    public class ReplaceLicenseCommandHandler
        (ILicenseRepository licenseRepository, IApplicationRepository applicationRepository, IApplicationTypeRepository applicationTypeRepository
        , IDriverRepository driverRepository)
        : ResponseHandler, IRequestHandler<ReplaceLicenseCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(ReplaceLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await licenseRepository.GetByIdAsync(request.OldLicenseId);
            if (license is null)
            {
                return NotFound<int?>("Licese not found");
            }
            enApplicationType ReplaceType = (request.IssueReason == enIssueReason.ReplacementDamaged) ?
                enApplicationType.ReplaceDamagedDrivingLicense :
                enApplicationType.ReplaceLostDrivingLicense;

            Task<AllEntities.Driver> DriverTask = driverRepository.GetByIdAsync(license.DriverID);
            Task<AllEntities.ApplicationType> ApplicationTypeTask = applicationTypeRepository.GetByIdAsync((int)ReplaceType);


            if (!license.IsActive)
            {
                return BadRequest<int?>("The license is inactive and cannot replaced.");
            }
            await Task.WhenAll(DriverTask, ApplicationTypeTask);

            if (DriverTask.Result == null || ApplicationTypeTask.Result == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for replacing.");
            }

            license.DriverInfo = DriverTask.Result;
            AllEntities.ApplicationType? applicationType = ApplicationTypeTask.Result;

            AllEntities.Application application = new();
            application.ApplicantPersonID = license.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;

            application.ApplicationTypeID = ReplaceType;

            application.ApplicationStatusID = enApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = applicationType.ApplicationFees;
            application.CreatedByUserID = request.CreatedUserId;

            if (!await applicationRepository.SaveAsync(application))
            {
                return Fail<int?>(null, "Error adding application");
            }

            AllEntities.License newLicese = new();
            newLicese.ApplicationID = application.Id;
            newLicese.DriverID = license.DriverID;
            newLicese.LicenseClassID = license.LicenseClassID;
            newLicese.IssueDate = DateTime.Now;
            newLicese.ExpirationDate = license.ExpirationDate;
            newLicese.Notes = license.Notes;
            newLicese.PaidFees = 0;
            newLicese.IsActive = true;
            newLicese.IssueReason = request.IssueReason;
            newLicese.CreatedByUserID = request.CreatedUserId;

            if (!await licenseRepository.SaveAsync(newLicese))
            {
                return Fail<int?>(null, "Error creating new License");
            }

            if (!await licenseRepository.DeactivateLicense(license.Id))
            {
                return Fail<int?>(null, "Error deactivating the old License");
            }

            return Created<int?>(newLicese.Id);
        }
    }
}
