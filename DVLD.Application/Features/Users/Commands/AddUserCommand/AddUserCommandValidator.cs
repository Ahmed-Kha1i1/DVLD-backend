using ApiLayer.Validators;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Users.Common.Validators;
using FluentValidation;

namespace DVLD.Application.Features.Users.Commands.AddUserCommand
{
    public class AddUserCommandvalidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandvalidator(IPersonRepository personRepository, IUserRepository userRepository)
        {
            RuleFor(user => user.Username)
                .Requeired().ValidateUsername()
                .MustAsync(async (Model, username, cancellation) => await userRepository.IsUsernameUnique(username))
                .WithMessage("Username must be unique");

            RuleFor(user => user.PersonID)
                .Requeired().ValidId()
                .MustAsync(async (Model, PersonId, cancellation) => await personRepository.IsPersonExists(PersonId)).WithMessage("No person found with ID: '{PropertyValue}'")
                .MustAsync(async (Model, PersonId, cancellation) => await userRepository.IsPersonIdUnique(PersonId)).WithMessage("This person is already associated with a user. Please choose another one.");


            RuleFor(user => user.Password).Requeired().ValidPassword();

        }
    }
}
