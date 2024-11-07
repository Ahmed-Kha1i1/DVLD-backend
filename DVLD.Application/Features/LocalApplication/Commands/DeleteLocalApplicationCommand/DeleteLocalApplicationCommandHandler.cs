using System.Net;


namespace DVLD.Application.Features.LocalApplication.Commands.DeleteLocalApplicationCommand
{
    public class DeleteLocalApplicationCommandHandler
        (ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository)
        : ResponseHandler, IRequestHandler<DeleteLocalApplicationCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(DeleteLocalApplicationCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = await localApplicationRepository.GetByIdAsync(request.LocalApplicationId);
            if (localApplication is null)
            {
                return NotFound<bool>("Local application not found");
            }

            if (!await localApplicationRepository.DeleteAsync(request.LocalApplicationId))
            {
                return Custom<bool>(HttpStatusCode.Conflict, false, $"Cannot delete local application with id {request.LocalApplicationId}");
            }

            if (!await applicationRepository.DeleteAsync(localApplication.ApplicationId))
            {
                return Custom<bool>(HttpStatusCode.Conflict, false, $"Cannot delete application with id {localApplication.ApplicationId}");
            }
            else
            {
                return Success<bool>(true, $"Local application with id {request.LocalApplicationId} has been deleted.");
            }
        }
    }
}
