
using AutoMapper;

namespace DVLD.Application.Features.Application.Queries.GetApplicationQuery
{
    public class GetApplicationQueryHandler(IApplicationRepository applicationRepository, IMapper mapper)
        : ResponseHandler, IRequestHandler<GetApplicationQuery, Response<GetApplicationQueryResponse>>
    {
        public async Task<Response<GetApplicationQueryResponse>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
        {
            var application = await applicationRepository.GetByIdAsync(request.applicationId);
            if (application is null)
            {
                return NotFound<GetApplicationQueryResponse>("Application not found");
            }

            return Success(mapper.Map<GetApplicationQueryResponse>(application));
        }
    }
}
