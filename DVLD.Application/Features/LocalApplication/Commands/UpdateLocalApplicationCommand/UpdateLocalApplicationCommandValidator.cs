namespace DVLD.Application.Features.LocalApplication.Commands.UpdateLocalApplicationCommand
{
    public class UpdateLocalApplicationCommandValidator : AbstractValidator<UpdateLocalApplicationCommand>
    {
        public UpdateLocalApplicationCommandValidator()
        {
            RuleFor(app => app.LicenseClassId).ValidId();
            RuleFor(app => app.UserId).ValidId();
            RuleFor(app => app.LocalApplicationId).ValidId();
        }
    }
}
