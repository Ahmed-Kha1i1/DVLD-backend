using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.License.Common.Models;
using MediatR;

namespace DVLD.Application.Features.License.Queries.GetLicenseQuery
{
    public class GetLicenseQueryHandler(ILicenseRepository licenseRepository) : ResponseHandler, IRequestHandler<GetLicenseQuery, Response<LicenseDTO>>
    {
        public async Task<Response<LicenseDTO>> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
        {
            var license = await licenseRepository.GetAsync(request.licenseId);
            if (license == null)
            {
                return NotFound<LicenseDTO>("License not found");
            }

            return Success(license);
        }
    }
}
