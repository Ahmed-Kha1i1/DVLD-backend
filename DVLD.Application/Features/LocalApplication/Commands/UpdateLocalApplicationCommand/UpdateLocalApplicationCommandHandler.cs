namespace DVLD.Application.Features.LocalApplication.Commands.UpdateLocalApplicationCommand
{
    public class UpdateLocalApplicationCommandHandler
        (ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository, ILicenseRepository licenseRepository)
        : ResponseHandler, IRequestHandler<UpdateLocalApplicationCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(UpdateLocalApplicationCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = await localApplicationRepository.GetByIdAsync(request.LocalApplicationId);

            if (localApplication is null)
            {
                return NotFound<bool>("Local application not found");
            }

            localApplication.ApplicationInfo = await applicationRepository.GetByIdAsync(localApplication.ApplicationId);
            if (localApplication.ApplicationInfo == null)
            {
                return Fail<bool>(false, "Error retrieving necessary information for canceling updaing application.");
            }
            var checkResult = await Check(localApplication, request);
            if (!checkResult.IsSuccess)
            {
                return checkResult.Response;
            }

            localApplication.ApplicationInfo.ApplicationDate = DateTime.Now;
            localApplication.ApplicationInfo.ApplicationTypeID = Domain.Common.Enums.enApplicationType.NewDrivingLicense;
            localApplication.ApplicationInfo.ApplicationStatusID = Domain.Common.Enums.enApplicationStatus.New;
            localApplication.ApplicationInfo.LastStatusDate = DateTime.Now;
            localApplication.ApplicationInfo.CreatedByUserID = request.UserId;

            if (!await applicationRepository.SaveAsync(localApplication.ApplicationInfo))
            {
                return Fail<bool>(false, "Error updating application");
            }

            localApplication.LicenseClassID = request.LicenseClassId;
            if (!await localApplicationRepository.SaveAsync(localApplication))
            {
                return Fail<bool>(false, "Error updating application");
            }

            return Success(true);
        }

        private async Task<(bool IsSuccess, Response<bool>? Response)> Check(AllEntities.LocalApplication localApplication, UpdateLocalApplicationCommand request)
        {
            if (localApplication.ApplicationInfo.ApplicationStatusID != Domain.Common.Enums.enApplicationStatus.New)
            {
                return (false, BadRequest<bool>("Only Application with new status can be updated"));
            }
            int? ActiveApplicationId = await applicationRepository.GetActiveApplicationId(localApplication.ApplicationInfo.ApplicantPersonID, request.LicenseClassId);
            if (ActiveApplicationId is not null && ActiveApplicationId != localApplication.ApplicationId)
            {
                return (false, BadRequest<bool>($"This person already have an active application for the selected class with id = {ActiveApplicationId}"));
            }

            bool IsLicenseExist = await licenseRepository.IsLicenseExist(localApplication.ApplicationInfo.ApplicantPersonID, request.LicenseClassId);
            if (IsLicenseExist)
            {
                return (false, BadRequest<bool>("Person already have a license with the same applied driving class, Choose diffrent driving class"));
            }

            return (true, null);
        }
    }
}
