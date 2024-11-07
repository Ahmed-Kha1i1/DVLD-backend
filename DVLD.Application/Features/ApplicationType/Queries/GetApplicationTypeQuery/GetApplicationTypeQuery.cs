using DVLD.Application.Common.Response;
using DVLD.Application.Features.ApplicationType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.ApplicationType.Queries.GetApplicationType
{
    public class GetApplicationTypeQuery : IRequest<Response<ApplicationTypeDTO>>
    {
        public int ApplicationtypeId { get; set; }

        public GetApplicationTypeQuery(int applicationtypeId)
        {
            ApplicationtypeId = applicationtypeId;
        }
    }
}
