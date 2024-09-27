using ApiLayer.Configurations;
using ApiLayer.DTOs.Person;
using ApiLayer.Services;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ApiLayer.Validators.Person
{
    public class AddPersonValidator : AbstractValidator<PersonForCreateDTO>
    {

        public AddPersonValidator(IOptions<ImagesOptions> imagesOptions, PersonService personService)
        {
            Include(new ModificationPersonValidator(imagesOptions));
            RuleFor(person => person.NationalNo).ValidateUniqueNationalNo(personService);
            RuleFor(person => person.Phone).ValidateUniqueEmail(personService);
            RuleFor(person => person.Email).ValidateUniqueEmail(personService);
        }
    }
}


