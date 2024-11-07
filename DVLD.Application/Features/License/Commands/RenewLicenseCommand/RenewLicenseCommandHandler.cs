using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.License.Commands.RenewLicenseCommand
{
    public class RenewLicenseCommandHandler
        (ILicenseRepository licenseRepository, IApplicationRepository applicationRepository, IApplicationTypeRepository applicationTypeRepository
        , IDriverRepository driverRepository, ILicenseClassRepository licenseClassRepository)
        : ResponseHandler, IRequestHandler<RenewLicenseCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(RenewLicenseCommand request, CancellationToken cancellationToken)
        {
            var oldLicense = await licenseRepository.GetByIdAsync(request.OldLicenseId);
            if (oldLicense == null)
            {
                return NotFound<int?>("License not found.");
            }
            Task<AllEntities.Driver> DriverTask = driverRepository.GetByIdAsync(oldLicense.DriverID);
            Task<AllEntities.LicenseClass> LicenseClassTask = licenseClassRepository.GetByIdAsync(oldLicense.LicenseClassID);
            Task<AllEntities.ApplicationType> ApplicationTypeTask = applicationTypeRepository.GetByIdAsync((int)enApplicationType.RenewDrivingLicense);

            if (!oldLicense.IsActive)
            {
                return BadRequest<int?>("The license is inactive and cannot renewed.");
            }
            if (oldLicense.ExpirationDate > DateTime.Now)
            {
                return BadRequest<int?>("License is not yet expired");
            }

            await Task.WhenAll(DriverTask, ApplicationTypeTask, LicenseClassTask);

            if (DriverTask.Result == null || LicenseClassTask.Result == null || ApplicationTypeTask.Result == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for renewal.");
            }


            oldLicense.DriverInfo = DriverTask.Result;
            oldLicense.LicenseClassInfo = LicenseClassTask.Result;
            AllEntities.ApplicationType? applicationType = ApplicationTypeTask.Result;

            AllEntities.Application application = new();
            application.ApplicantPersonID = oldLicense.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = enApplicationType.RenewDrivingLicense;
            application.ApplicationStatusID = enApplicationStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = applicationType.ApplicationFees;
            application.CreatedByUserID = request.CreatedUserId;

            if (!await applicationRepository.SaveAsync(application))
            {
                return Fail<int?>(null, "Error adding application");
            }

            AllEntities.License NewLicense = new();
            NewLicense.ApplicationID = application.Id;
            NewLicense.DriverID = oldLicense.DriverID;
            NewLicense.LicenseClassID = oldLicense.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = oldLicense.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = request.Notes;
            NewLicense.PaidFees = oldLicense.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = request.CreatedUserId;

            if (!await licenseRepository.SaveAsync(NewLicense))
            {
                return Fail<int?>(null, "Error renewing License");
            }

            if (!await licenseRepository.DeactivateLicense(oldLicense.Id))
            {
                return Fail<int?>(null, "Error deactivating the old License");
            }

            return Created<int?>(NewLicense.Id);
        }
    }
}
