using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using MediatR;
using System.Net;

namespace DVLD.Application.Features.Users.Commands.DeleteUserCommand
{
    public class DeleteUserCommandHandler(IUserRepository userRepository) : ResponseHandler, IRequestHandler<DeleteUserCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userExists = await userRepository.IsUserExistByUserId(request.UserId);

            if (!userExists)
            {
                return NotFound<int?>("User not found");
            }

            if (await userRepository.DeleteAsync(request.UserId))
            {
                return Success<int?>(request.UserId, $"User with id {request.UserId} has been deleted.");
            }
            else
            {
                return Custom<int?>(HttpStatusCode.Conflict, null, $"Cannot delete user with id {request.UserId}");
            }
        }
    }
}
