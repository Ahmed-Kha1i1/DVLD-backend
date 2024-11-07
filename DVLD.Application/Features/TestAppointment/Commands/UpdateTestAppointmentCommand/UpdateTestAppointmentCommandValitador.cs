namespace DVLD.Application.Features.TestAppointment.Commands.UpdateTestAppointmentCommand
{
    public class UpdateTestAppointmentCommandValitador : AbstractValidator<UpdateTestAppointmentCommand>
    {
        public UpdateTestAppointmentCommandValitador()
        {
            RuleFor(x => x.CreatedUserId).ValidId();
            RuleFor(x => x.TestAppointmentId).ValidId();
            RuleFor(x => x.AppointmentDate).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("{PropertyValue} Appointment date must be greater than or equeal to today.");
        }
    }
}
