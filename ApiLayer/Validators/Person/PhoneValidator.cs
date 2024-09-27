using ApiLayer.Validators;
using FluentValidation;

public class PhoneValidator : AbstractValidator<string>
{
    public PhoneValidator()
    {
        RuleFor(Phone => Phone)
              .Requeired("Phone")
              .Matches(ValidationRegex.PhonePattern).WithMessage("Phone is not valid");
    }
}
