using ApiLayer.Validators;
using DVLD.Application.Models;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DVLD.Application.Features.People.Commands.ModificationPerson
{
    public class ModificationPersonCommandValidator : AbstractValidator<ModificationPersonCommand>
    {
        private readonly IOptionsSnapshot<ImagesOptions> _imagesOptions;
        public ModificationPersonCommandValidator(IOptionsSnapshot<ImagesOptions> imagesOptions)
        {
            _imagesOptions = imagesOptions;

            ConfigureValidationRules();
        }

        private void ConfigureValidationRules()
        {
            RuleFor(person => person.ImageFile).SetValidator(new ImageValidator(_imagesOptions));
            SetNameRules();
            AutherRules();

            RuleFor(person => person.Address)
              .Requeired()
              .MaximumLength(500).WithMessage("The length of address must be 500 characters or fewer");
        }

        private void SetNameRules()
        {
            RuleFor(person => person.FirstName)
                .Requeired()
                .MaximumLength(20)
                .MatchesNamePattern();

            RuleFor(person => person.SecondName)
                .Requeired()
                .MaximumLength(20)
                .MatchesNamePattern();

            RuleFor(person => person.ThirdName)
                .MaximumLength(20)
                .MatchesNamePattern();

            RuleFor(person => person.LastName)
                .Requeired()
                .MaximumLength(20)
                .MatchesNamePattern();
        }
        private void AutherRules()
        {

            RuleFor(person => person.DateOfBirth)
                 .InclusiveBetween(DateOnly.FromDateTime(DateTime.Now.AddYears(-100)), DateOnly.FromDateTime(DateTime.Now.AddYears(-18)))
                .WithMessage("Date of birth must be between 18 and 100 years ago")
                .Requeired();

            RuleFor(person => person.Gender)
                .Must(x => new[] { "male", "female" }.Contains(x.ToLower()))
                .WithMessage("Gender must be either 'Male'or 'Female'");

            RuleFor(person => person.NationalityCountryID)
                .Requeired()
                .GreaterThan(0).WithMessage("Nationality Country ID is not valid");
        }
    }
}
