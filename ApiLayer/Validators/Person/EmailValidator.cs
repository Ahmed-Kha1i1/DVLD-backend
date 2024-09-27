using ApiLayer.Validators;
using FluentValidation;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(Email => Email)
                .Requeired("Email")
                .EmailAddress().WithMessage("Invalid email format");
    }
}
