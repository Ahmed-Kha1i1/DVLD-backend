using DVLD.Application.Features.LocalApplication.Common.Models;

namespace DVLD.Application.Features.LocalApplication.Queries.GetLocalApplicationQuery
{
    public class GetLocalApplicationQueryHandler(ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository)
        : ResponseHandler,
          IRequestHandler<GetLocalApplicationQuery, Response<LocalApplicationDTO>>
        , IRequestHandler<GetLocalApplicationPrefQuery, Response<LocalApplicationPrefDTO>>
    {
        public async Task<Response<LocalApplicationDTO>> Handle(GetLocalApplicationQuery request, CancellationToken cancellationToken)
        {
            var LocalApplication = await localApplicationRepository.GetOverviewAsync(request.LocalApplicationId);
            if (LocalApplication == null)
            {
                return NotFound<LocalApplicationDTO>("Local application not found");
            }
            LocalApplication.basicApplication = await applicationRepository.GetOverviewAsync(LocalApplication.ApplicationId);
            if (LocalApplication.basicApplication == null)
            {
                return Fail<LocalApplicationDTO>(null, "Basic application not found.");
            }
            return Success(LocalApplication);
        }

        public async Task<Response<LocalApplicationPrefDTO>> Handle(GetLocalApplicationPrefQuery request, CancellationToken cancellationToken)
        {
            var LocalApplication = await localApplicationRepository.GetPref(request.LocalApplicationId);
            if (LocalApplication == null)
            {
                return NotFound<LocalApplicationPrefDTO>("Local application not found");
            }
            return Success(LocalApplication);
        }
    }
}
