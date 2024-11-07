namespace DVLD.Application.Features.Users.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordCommandHandler(IUserRepository userRepository) : ResponseHandler, IRequestHandler<UpdatePasswordCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            if (!await userRepository.IsPasswordValid(request.UserId, request.OldPassword))
            {
                return BadRequest<bool>("The old password you provided is incorrect.");
            }

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
