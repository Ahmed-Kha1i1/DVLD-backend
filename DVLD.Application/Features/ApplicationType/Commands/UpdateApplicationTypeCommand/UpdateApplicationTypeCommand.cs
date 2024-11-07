using DVLD.Application.Common.Response;
using DVLD.Application.Features.ApplicationType.Common.Models;
using MediatR;

namespace DVLD.Application.Features.ApplicationType.Commands.UpdateApplicationTypeCommand
{
    public class UpdateApplicationTypeCommand : IRequest<Response<ApplicationTypeDTO>>
    {
        public int ApplicationTypeId { get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }

    }
}
