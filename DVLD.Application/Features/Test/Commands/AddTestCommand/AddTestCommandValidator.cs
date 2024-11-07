namespace DVLD.Application.Features.Test.Commands.AddTestCommand
{
    public class AddTestCommandValidator : AbstractValidator<AddTestCommand>
    {
        public AddTestCommandValidator()
        {
            RuleFor(x => x.TestAppointmentId).ValidId();
            RuleFor(x => x.CreatedUserId).ValidId();
        }
    }
}
