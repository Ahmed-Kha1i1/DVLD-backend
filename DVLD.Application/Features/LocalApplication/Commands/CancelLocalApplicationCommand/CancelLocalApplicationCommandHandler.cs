

namespace DVLD.Application.Features.LocalApplication.Commands.CancelLocalApplicationCommand
{
    public class CancelLocalApplicationCommandHandler
          (ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository)
        : ResponseHandler, IRequestHandler<CancelLocalApplicationCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(CancelLocalApplicationCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = await localApplicationRepository.GetByIdAsync(request.LocalApplicationId);
            if (localApplication is null)
            {
                return NotFound<bool>("Local application not found");
            }

            localApplication.ApplicationInfo = await applicationRepository.GetByIdAsync(localApplication.ApplicationId);
            if (localApplication.ApplicationInfo == null)
            {
                return Fail<bool>(false, "Error retrieving necessary information for canceling application.");
            }

            if (localApplication.ApplicationInfo.ApplicationStatusID != Domain.Common.Enums.enApplicationStatus.New)
            {
                return BadRequest<bool>("Only Application with new status can be canceled");
            }

            if (!await applicationRepository.Cancel(localApplication.ApplicationId))
            {
                return Custom<bool>(System.Net.HttpStatusCode.Conflict, false, "Could not cancel applicatoin.");
            }

            return Success<bool>(true, "Application Cancelled Successfully");
        }
    }
}
