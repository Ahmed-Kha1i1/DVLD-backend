using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;

namespace DVLD.Application.Features.LocalApplication.Commands.IssueLocalApplicationCommand
{
    public class IssueLocalApplicationCommandHandler : ResponseHandler, IRequestHandler<IssueLocalApplicationCommand, Response<int?>>
    {
        private readonly ILocalApplicationRepository _localApplicationRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILicenseRepository _licenseRepository;
        private readonly ILicenseClassRepository _licenseClassRepository;
        private readonly IDriverRepository _driverRepository;

        public IssueLocalApplicationCommandHandler(
            ILocalApplicationRepository localApplicationRepository,
            IApplicationRepository applicationRepository,
            ILicenseRepository licenseRepository,
            ILicenseClassRepository licenseClassRepository,
            IDriverRepository driverRepository)
        {
            _localApplicationRepository = localApplicationRepository;
            _applicationRepository = applicationRepository;
            _licenseRepository = licenseRepository;
            _licenseClassRepository = licenseClassRepository;
            _driverRepository = driverRepository;
        }

        public async Task<Response<int?>> Handle(IssueLocalApplicationCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = await _localApplicationRepository.GetByIdAsync(request.LocalApplicationId);
            if (localApplication is null)
            {
                return NotFound<int?>("Local application not found");
            }

            Task<AllEntities.Application?> applicationTask = _applicationRepository.GetByIdAsync(localApplication.ApplicationId);
            Task<LicenseClass?> licenseClassTask = _licenseClassRepository.GetByIdAsync(localApplication.LicenseClassID);

            await Task.WhenAll(applicationTask, licenseClassTask);
            if (applicationTask.Result == null || licenseClassTask.Result == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for canceling issueing.");
            }

            localApplication.ApplicationInfo = applicationTask.Result;
            localApplication.LicenseClassInfo = licenseClassTask.Result;

            if (localApplication.ApplicationInfo.ApplicationStatusID != enApplicationStatus.New)
            {
                return BadRequest<int?>("Only Application with new status can be issued");
            }

            int? driverId = await GetOrCreateDriverIdAsync(localApplication.ApplicationInfo.ApplicantPersonID, request.userId);
            if (driverId is null)
            {
                return Fail<int?>(null, "Error adding driver.");
            }

            var licenseId = await CreateLicenseAsync(localApplication, driverId ?? 0, request.Notes, request.userId);
            if (licenseId is null)
            {
                return Fail<int?>(null, "Error adding license.");
            }

            if (!await _applicationRepository.SetComplete(localApplication.ApplicationId))
            {
                return Fail<int?>(null, "Error  updating status to complete.");
            }
            return Created(licenseId);
        }

        private async Task<int?> GetOrCreateDriverIdAsync(int personId, int userId)
        {
            var driver = await _driverRepository.GetBypersonId(personId);
            if (driver == null)
            {
                var newDriver = new AllEntities.Driver
                {
                    PersonID = personId,
                    CreatedByUserID = userId
                };

                if (!await _driverRepository.SaveAsync(newDriver))
                {
                    return null;
                }
                return newDriver.Id;
            }

            return driver.DriverID;
        }

        private async Task<int?> CreateLicenseAsync(AllEntities.LocalApplication localApplication, int driverId, string? notes, int userId)
        {
            var license = new AllEntities.License
            {
                ApplicationID = localApplication.ApplicationId,
                DriverID = driverId,
                LicenseClassID = localApplication.LicenseClassID,
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(localApplication.LicenseClassInfo.DefaultValidityLength),
                Notes = notes,
                PaidFees = localApplication.LicenseClassInfo.ClassFees,
                IsActive = true,
                IssueReason = enIssueReason.FirstTime,
                CreatedByUserID = userId
            };

            if (!await _licenseRepository.SaveAsync(license))
            {
                return null;
            }

            return license.Id;
        }
    }
}
