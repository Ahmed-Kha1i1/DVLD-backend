using DataLayerCore.User;
using FluentValidation;

namespace ApiLayer.Validators.Person
{
    public class ModificationUserValidator : AbstractValidator<UserForModificationDTO>
    {
        public ModificationUserValidator()
        {
            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(user => user.PersonID)
                .Requeired("Person id");

            RuleFor(user => user.UserName)
                .Requeired("Username")
                .MaximumLength(20)
                .MatchesNamePattern("Username");
        }

    }
}
