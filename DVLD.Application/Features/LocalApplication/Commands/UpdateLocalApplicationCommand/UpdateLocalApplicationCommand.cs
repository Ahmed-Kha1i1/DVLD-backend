using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.LocalApplication.Commands.UpdateLocalApplicationCommand
{
    public class UpdateLocalApplicationCommand : IRequest<Response<bool>>
    {
        public int LicenseClassId { get; set; }
        public int LocalApplicationId { get; set; }
        public int UserId { get; set; }
    }
}
