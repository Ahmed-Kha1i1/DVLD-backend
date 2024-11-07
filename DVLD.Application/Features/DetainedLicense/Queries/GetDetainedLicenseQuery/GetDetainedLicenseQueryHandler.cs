
using AutoMapper;

namespace DVLD.Application.Features.DetainedLicense.Queries.GetDetainedLicenseQuery
{
    public class GetDetainedLicenseQueryHandler(IDetainedLicenseRepository detainedLicenseRepository, IMapper mapper)
        : ResponseHandler, IRequestHandler<GetDetainedLicenseQuery, Response<GetDetainedLicenseQueryResponse>>
         , IRequestHandler<GetDetainedLicenseByLicenseIdQuery, Response<GetDetainedLicenseQueryResponse>>
    {
        public async Task<Response<GetDetainedLicenseQueryResponse>> Handle(GetDetainedLicenseQuery request, CancellationToken cancellationToken)
        {
            var detainedLicense = await detainedLicenseRepository.GetByIdAsync(request.DetainId);
            if (detainedLicense is null)
            {
                return NotFound<GetDetainedLicenseQueryResponse>("Detained license not found");
            }

            return Success(mapper.Map<GetDetainedLicenseQueryResponse>(detainedLicense));
        }

        public async Task<Response<GetDetainedLicenseQueryResponse>> Handle(GetDetainedLicenseByLicenseIdQuery request, CancellationToken cancellationToken)
        {
            var detainedLicense = await detainedLicenseRepository.GetByLicenseIdAsync(request.LicenseId);
            if (detainedLicense is null)
            {
                return NotFound<GetDetainedLicenseQueryResponse>("Detained license not found");
            }

            return Success(mapper.Map<GetDetainedLicenseQueryResponse>(detainedLicense));
        }
    }
}
