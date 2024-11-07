using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.LocalApplication.Commands.AddLocalApplicationCommand
{
    public class AddLocalApplicationCommandHandler
        (ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository,
         IApplicationTypeRepository applicationTypeRepository)
        : ResponseHandler, IRequestHandler<AddLocalApplicationCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(AddLocalApplicationCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = new();
            localApplication.ApplicationInfo = new();

            AllEntities.ApplicationType? applicationType = await applicationTypeRepository.GetByIdAsync((int)enApplicationType.NewDrivingLicense);

            if (applicationType == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for adding adding local application.");
            }

            localApplication.ApplicationInfo.ApplicantPersonID = request.PersonId;
            localApplication.ApplicationInfo.ApplicationDate = DateTime.Now;
            localApplication.ApplicationInfo.ApplicationTypeID = enApplicationType.NewDrivingLicense;
            localApplication.ApplicationInfo.ApplicationStatusID = enApplicationStatus.New;
            localApplication.ApplicationInfo.LastStatusDate = DateTime.Now;
            localApplication.ApplicationInfo.PaidFees = applicationType.ApplicationFees;
            localApplication.ApplicationInfo.CreatedByUserID = request.UserId;

            if (!await applicationRepository.SaveAsync(localApplication.ApplicationInfo))
            {
                return Fail<int?>(null, "Error adding application");
            }
            localApplication.ApplicationId = localApplication.ApplicationInfo.Id;
            localApplication.LicenseClassID = request.LicenseClassId;
            if (!await localApplicationRepository.SaveAsync(localApplication))
            {
                return Fail<int?>(null, "Error adding application");
            }

            return Created<int?>(localApplication.Id);
        }
    }
}
