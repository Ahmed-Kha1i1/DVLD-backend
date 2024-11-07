namespace DVLD.Application.Features.TestAppointment.Commands.UpdateTestAppointmentCommand
{
    public class UpdateTestAppointmentCommandHandler(ITestAppointmentRepository testAppointmentRepository) : ResponseHandler, IRequestHandler<UpdateTestAppointmentCommand, Response<bool>>
    {
        public async Task<Response<bool>> Handle(UpdateTestAppointmentCommand request, CancellationToken cancellationToken)
        {
            var testAppointmnet = await testAppointmentRepository.GetByIdAsync(request.TestAppointmentId);
            if (testAppointmnet is null)
            {
                return NotFound<bool>("Test appointment not found");
            }

            if (testAppointmnet.IsLocked)
            {
                return BadRequest<bool>("Person already sat for the test, appointment loacked.");
            }

            testAppointmnet.AppointmentDate = request.AppointmentDate;
            testAppointmnet.CreatedByUserID = request.CreatedUserId;

            if (!await testAppointmentRepository.SaveAsync(testAppointmnet))
            {
                return Fail(false, "Error updating test appointment");
            }

            return Success(true);
        }
    }
}
