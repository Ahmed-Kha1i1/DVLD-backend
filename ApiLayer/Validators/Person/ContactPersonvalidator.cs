using ApiLayer.DTOs.Person;
using ApiLayer.Services;
using FluentValidation;

public class ContactPersonvalidator : AbstractValidator<ContactPersonDTO>
{
    public ContactPersonvalidator(PersonService personService, IHttpContextAccessor accessor)
    {
        RuleFor(contactPerson => contactPerson.Email).SetValidator(new EmailValidator()).ValidateUniqueEmail(personService, accessor);
        RuleFor(contactPerson => contactPerson.Phone).SetValidator(new PhoneValidator()).ValidateUniquePhone(personService, accessor);
        RuleFor(contactPerson => contactPerson.Address).SetValidator(new AddressValidator()).ValidateUniqueNationalNo(personService, accessor);
    }
}