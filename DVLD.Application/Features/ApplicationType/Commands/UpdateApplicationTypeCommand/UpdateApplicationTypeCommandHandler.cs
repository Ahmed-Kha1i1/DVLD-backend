using AutoMapper;
using DVLD.Application.Features.ApplicationType.Common.Models;

namespace DVLD.Application.Features.ApplicationType.Commands.UpdateApplicationTypeCommand
{
    public class UpdateApplicationTypeCommandHandler(IApplicationTypeRepository applicationTypeRepository, IMapper mapper) : ResponseHandler, IRequestHandler<UpdateApplicationTypeCommand, Response<ApplicationTypeDTO>>
    {
        public async Task<Response<ApplicationTypeDTO>> Handle(UpdateApplicationTypeCommand request, CancellationToken cancellationToken)
        {
            var ApplicationType = await applicationTypeRepository.GetByIdAsync(request.ApplicationTypeId);

            if (ApplicationType == null)
            {
                return NotFound<ApplicationTypeDTO>("Application type not found");
            }
            mapper.Map(request, ApplicationType);

            if (!await applicationTypeRepository.SaveAsync(ApplicationType))
            {
                return Fail<ApplicationTypeDTO>(null, "Error updating application type");
            }

            return Success(mapper.Map<ApplicationTypeDTO>(ApplicationType));
        }
    }
}
