using DVLD.Application.Common.Response;
using DVLD.Application.Features.Users.Common.Models;
using MediatR;

namespace DVLD.Application.Features.Users.Commands.AddUserCommand
{
    public class AddUserCommand : ModificationUserDTO, IRequest<Response<int?>>
    {
        public string Password { get; set; }
    }
}
