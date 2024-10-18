using DVLD.Application.Common.Response;
using DVLD.Application.Features.Users.Common.Models;
using MediatR;

namespace DVLD.Application.Features.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : ModificationUserDTO, IRequest<Response<UserDTO>>
    {
        public int UserId { get; set; }
    }
}
