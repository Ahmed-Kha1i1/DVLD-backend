using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Users.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommand : IRequest<Response<bool>>
    {
        public string NewPassword { get; set; }
        public int UserId { get; set; }
    }
}
