using AutoMapper;
using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.ApplicationType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.ApplicationType.Queries.GetApplicationType
{
    public class GetApplicationTypeQueryHandler(IApplicationTypeRepository applicationTypeRepository, IMapper mapper) : ResponseHandler, IRequestHandler<GetApplicationTypeQuery, Response<ApplicationTypeDTO>>
    {
        public async Task<Response<ApplicationTypeDTO>> Handle(GetApplicationTypeQuery request, CancellationToken cancellationToken)
        {
            var TestType = await applicationTypeRepository.GetByIdAsync(request.ApplicationtypeId);
            if (TestType == null)
            {
                return NotFound<ApplicationTypeDTO>("Application type not found");
            }

            return Success(mapper.Map<ApplicationTypeDTO>(TestType));
        }
    }
}
