

using AutoMapper;

namespace DVLD.Application.Features.InternationalLicense.Queries.GetInternationalLicenseApplicationQuery
{
    public class GetInternationalLicenseApplicationQueryHandler
        (IInternationalLicenseRepository internationalLicenseRepository, IApplicationRepository applicationRepository, IMapper mapper) :
        ResponseHandler, IRequestHandler<GetInternationalLicenseApplicationQuery, Response<GetInternationalLicenseApplicationQueryResponse>>
    {
        public async Task<Response<GetInternationalLicenseApplicationQueryResponse>> Handle(GetInternationalLicenseApplicationQuery request, CancellationToken cancellationToken)
        {
            var internationalLicense = await internationalLicenseRepository.GetByIdAsync(request.InternationalLicenseId);
            if (internationalLicense == null)
            {
                return NotFound<GetInternationalLicenseApplicationQueryResponse>("International license not found");

            }
            internationalLicense.ApplicationInfo = await applicationRepository.GetByIdAsync(internationalLicense.ApplicationID);
            if (internationalLicense.ApplicationInfo is null)
            {
                return NotFound<GetInternationalLicenseApplicationQueryResponse>("Basic application not found.");
            }
            return Success(mapper.Map<GetInternationalLicenseApplicationQueryResponse>(internationalLicense));
        }
    }
}
