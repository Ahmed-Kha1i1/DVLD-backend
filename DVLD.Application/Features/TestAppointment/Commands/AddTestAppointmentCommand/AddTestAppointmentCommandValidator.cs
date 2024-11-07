namespace DVLD.Application.Features.TestAppointment.Commands.AddTestAppointmentCommand
{
    public class AddTestAppointmentCommandValidator : AbstractValidator<AddTestAppointmentCommand>
    {

        public AddTestAppointmentCommandValidator(ILocalApplicationRepository localApplicationRepository)
        {

            RuleFor(x => x.LocalApplicationId).ValidId();
            RuleFor(x => x.CreatedUserId).ValidId();
            RuleFor(x => x.TestTypeId).IsInEnum();
            RuleFor(x => x.AppointmentDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("Appointment date must be greater than or equeal to today.");

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) => !await localApplicationRepository.IsThereAnActiveScheduledTest(command.LocalApplicationId, (int)command.TestTypeId))
                .WithMessage("Person Already have an active appointment for this test, You cannot add new appointment")
                .MustAsync(async (command, cancellation) => await localApplicationRepository.DoesPassPreviousTest(command.LocalApplicationId, command.TestTypeId))
                .WithMessage("Cannot Sechule, Previous test should be passed first")
                .MustAsync(async (command, cancellation) => !await localApplicationRepository.DoesPassTestType(command.LocalApplicationId, (int)command.TestTypeId))
                .WithMessage("This person already passed this test before, you can only retake faild test");
        }

    }
}
