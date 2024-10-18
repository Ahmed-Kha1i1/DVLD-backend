using DVLD.Application.Common.Response;
using MediatR;

namespace DVLD.Application.Features.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommand : IRequest<Response<int?>>
    {
        public int UserId { get; set; }
        public DeleteUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
