using AutoMapper;

namespace DVLD.Application.Features.Users.Commands.UpdateUserCommand
{
    public class UpdateUserCommandHandler(IPersonRepository personRepository, ICountryRepository countryRepository, IUserRepository userRepository, IMapper mapper) : ResponseHandler, IRequestHandler<UpdateUserCommand, Response<UserDTO>>
    {
        public async Task<Response<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound<UserDTO>("User not found");
            }

            mapper.Map(request, user);

            if (!await userRepository.SaveAsync(user))
            {
                return Fail<UserDTO>(null, "Error updating user");
            }
            user.PersonInfo = await personRepository.GetByIdAsync(user.PersonID);
            if (user.PersonInfo == null)
            {
                return NotFound<UserDTO>("Person not found");
            }
            user.PersonInfo.CountryInfo = await countryRepository.GetByIdAsync(user.PersonInfo.NationalityCountryID);
            if (user.PersonInfo.CountryInfo is null)
            {
                return NotFound<UserDTO>("Country not found");
            }

            var mappedUser = mapper.Map<UserDTO>(user);
            return Success(mappedUser);
        }
    }
}
