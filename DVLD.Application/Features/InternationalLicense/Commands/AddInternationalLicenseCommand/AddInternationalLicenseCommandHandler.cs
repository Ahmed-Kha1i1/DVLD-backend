using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.InternationalLicense.Commands.AddInternationalLicenseCommand
{
    public class AddInternationalLicenseCommandHandler
        (ILicenseRepository licenseRepository, IInternationalLicenseRepository internationalLicenseRepository,
        IApplicationRepository applicationRepository, IApplicationTypeRepository applicationTypeRepository, IDriverRepository driverRepository)
        : ResponseHandler, IRequestHandler<AddInternationalLicenseCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(AddInternationalLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await licenseRepository.GetByIdAsync(request.LicenseId);

            Task<AllEntities.Driver> driverTask = driverRepository.GetByIdAsync(license.DriverID);
            Task<AllEntities.ApplicationType> ApplicationTypeTask = applicationTypeRepository.GetByIdAsync((int)enApplicationType.NewInternationalLicense);

            if (license.DriverInfo == null)
            {
                return NotFound<int?>("Error retrieving necessary information for Adding international license.");
            }
            if (license == null)
            {
                return NotFound<int?>($"License with ID {request.LicenseId} was not found.");
            }

            if (license.LicenseClassID != 3)
            {
                return BadRequest<int?>("The license must be of Class 3 to qualify for an international license.");
            }


            if (license.ExpirationDate <= DateTime.Now)
            {
                return BadRequest<int?>("The license has expired and is not eligible for an international license.");
            }


            if (!license.IsActive)
            {
                return BadRequest<int?>("The license is inactive and cannot be used to issue an international license.");
            }

            int? activeLicenseId = await internationalLicenseRepository.GetActiveInternationalLicenseIDByDriverID(license.DriverID);
            if (activeLicenseId is not null)
            {
                return BadRequest<int?>($"Driver already holds an active international license with ID {activeLicenseId}.");
            }
            await Task.WhenAll(driverTask, ApplicationTypeTask);

            if (driverTask.Result == null || ApplicationTypeTask.Result == null)
            {
                return NotFound<int?>("Error retrieving necessary information for Adding international license.");
            }

            license.DriverInfo = driverTask.Result;
            AllEntities.ApplicationType? applicationType = ApplicationTypeTask.Result;

            AllEntities.InternationalLicense InternationalLicense = new();
            InternationalLicense.ApplicationInfo = new();

            InternationalLicense.ApplicationInfo.ApplicantPersonID = license.DriverInfo.PersonID;
            InternationalLicense.ApplicationInfo.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationInfo.ApplicationStatusID = enApplicationStatus.Completed;
            InternationalLicense.ApplicationInfo.ApplicationTypeID = enApplicationType.NewInternationalLicense;
            InternationalLicense.ApplicationInfo.LastStatusDate = DateTime.Now;
            InternationalLicense.ApplicationInfo.PaidFees = applicationType.ApplicationFees;
            InternationalLicense.ApplicationInfo.CreatedByUserID = request.CreatedUserId;


            if (!await applicationRepository.SaveAsync(InternationalLicense.ApplicationInfo))
            {
                return Fail<int?>(null, "Error adding application");
            }

            InternationalLicense.DriverID = license.DriverID;
            InternationalLicense.ApplicationID = InternationalLicense.ApplicationInfo.Id;
            InternationalLicense.IssuedUsingLocalLicenseID = license.Id;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.CreatedByUserID = request.CreatedUserId;


            if (!await internationalLicenseRepository.SaveAsync(InternationalLicense))
            {
                return Fail<int?>(null, "Error adding international license");
            }

            return Created<int?>(InternationalLicense.Id);
        }
    }
}
