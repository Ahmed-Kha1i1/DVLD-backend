using DVLD.Application.Common.Response;
using DVLD.Application.Features.License.Common.Models;
using MediatR;

namespace DVLD.Application.Features.License.Queries.GetLicenseQuery
{
    public class GetLicenseQuery : IRequest<Response<LicenseDTO>>
    {
        public int licenseId { get; set; }

        public GetLicenseQuery(int licenseId)
        {
            this.licenseId = licenseId;
        }
    }
}

