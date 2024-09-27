using ApiLayer.Validators;
using FluentValidation;

public class NationalNoValidator : AbstractValidator<string>
{
    public NationalNoValidator()
    {
        RuleFor(NationalNo => NationalNo)
             .Requeired("National number")
             .MaximumLength(20)
             .Matches(ValidationRegex.NationalNoPattern).WithMessage("National number can only contain letters, numbers, and hyphens");
    }
}