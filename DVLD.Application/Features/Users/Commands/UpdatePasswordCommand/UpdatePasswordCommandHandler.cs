using DVLD.Application.Common.Response;
using DVLD.Application.Contracts.Persistence;
using MediatR;

namespace DVLD.Application.Features.Users.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommandHandler(IUserRepository userRepository) : ResponseHandler, IRequestHandler<UpdatePasswordCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            if (await userRepository.UpdatePassword(request.UserId, request.NewPassword))
            {
                return Success(true);
            }
            else
            {
                return Fail(false, "Error updating password");
            }
        }
    }
}
