using ApiLayer.Configurations;
using ApiLayer.DTOs.Person;
using ApiLayer.Services;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ApiLayer.Validators.Person
{
    public class UpdatePersonValidator : AbstractValidator<PersonForUpdateDTO>
    {

        public UpdatePersonValidator(IOptions<ImagesOptions> imagesOptions, IHttpContextAccessor accessor, PersonService personService)
        {
            Include(new ModificationPersonValidator(imagesOptions));
            RuleFor(person => person.NationalNo).ValidateUniqueEmail(personService, accessor);
            RuleFor(person => person.Phone).ValidateUniquePhone(personService, accessor);
            RuleFor(person => person.Email).ValidateUniqueEmail(personService, accessor);
        }
    }
}
