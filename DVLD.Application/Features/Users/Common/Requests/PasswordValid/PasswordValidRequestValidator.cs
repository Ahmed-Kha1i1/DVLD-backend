namespace DVLD.Application.Features.Users.Common.Requests.PasswordValid
{
    public class PasswordValidRequestValidator : AbstractValidator<PasswordValidRequest>
    {
        public PasswordValidRequestValidator()
        {
            RuleFor(x => x.UserId).ValidId();
            RuleFor(x => x.CurrentPassword).Requeired();
        }
    }
}
