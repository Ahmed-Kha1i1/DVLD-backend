using ApiLayer.Validators;
using FluentValidation;

public class AddressValidator : AbstractValidator<string>
{
    public AddressValidator()
    {
        RuleFor(Address => Address)
               .Requeired("Address")
               .MaximumLength(500).WithMessage("The length of address must be 500 characters or fewer");
    }
}
