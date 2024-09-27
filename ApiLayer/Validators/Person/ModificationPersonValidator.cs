using ApiLayer.Configurations;
using ApiLayer.DTOs.Person;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ApiLayer.Validators.Person
{
    public class ModificationPersonValidator : AbstractValidator<PersonForModificationDTO>
    {
        private readonly IOptions<ImagesOptions> _imagesOptions;
        public ModificationPersonValidator(IOptions<ImagesOptions> imagesOptions)
        {
            _imagesOptions = imagesOptions;

            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            SetNameRules();
            AutherRoles();
            RuleFor(person => person.NationalNo).SetValidator(new NationalNoValidator());
            RuleFor(person => person.Email).SetValidator(new EmailValidator());
            RuleFor(person => person.Phone).SetValidator(new PhoneValidator());
            RuleFor(person => person.Address).SetValidator(new AddressValidator());
            RuleFor(person => person.ImageFile).SetValidator(new ImageValidator(_imagesOptions));
        }

        private void SetNameRules()
        {
            RuleFor(person => person.FirstName)
                .Requeired("First name")
                .MaximumLength(20)
                .MatchesNamePattern("First name");

            RuleFor(person => person.SecondName)
                .Requeired("Second name")
                .MaximumLength(20)
                .MatchesNamePattern("Second name");

            RuleFor(person => person.ThirdName)
                .MaximumLength(20)
                .MatchesNamePattern("Third Name");

            RuleFor(person => person.LastName)
                .Requeired("Last name")
                .MaximumLength(20)
                .MatchesNamePattern("Last name");
        }
        private void AutherRoles()
        {

            RuleFor(person => person.DateOfBirth)
                .InclusiveBetween(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(-18)).WithMessage("Date of birth must be between 18 and 100 years ago")
                .Requeired("Date of birth ");

            RuleFor(person => person.Gender)
                .IsInEnum().WithMessage("Gender is not valid");

            RuleFor(person => person.NationalityCountryID)
                .Requeired("Nationality Country ID")
                .GreaterThan(0).WithMessage("Nationality Country ID is not valid");
        }

    }
}
