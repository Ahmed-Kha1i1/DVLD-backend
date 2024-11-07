
using AutoMapper;

namespace DVLD.Application.Features.TestAppointment.Queries.GetTestAppointmentQuery
{
    public class GetTestAppointmentQueryHandler(ITestAppointmentRepository testAppointmentRepository, IApplicationRepository applicationRepository, IMapper mapper) : ResponseHandler, IRequestHandler<GetTestAppointmentQuery, Response<GetTestAppointmentQueryResponse>>
    {
        public async Task<Response<GetTestAppointmentQueryResponse>> Handle(GetTestAppointmentQuery request, CancellationToken cancellationToken)
        {
            var testAppointment = await testAppointmentRepository.GetByIdAsync(request.TestAppointmentId);
            if (testAppointment == null)
            {
                return NotFound<GetTestAppointmentQueryResponse>("Test Apointment not found");
            }

            var resposne = mapper.Map<GetTestAppointmentQueryResponse>(testAppointment);
            if (testAppointment.RetakeTestApplicationID != null)
            {
                resposne.RetakeTestApplication = new();
                resposne.RetakeTestApplication.PaidFees = await applicationRepository.GetpPaidFees(testAppointment.RetakeTestApplicationID ?? 0);
                resposne.RetakeTestApplication.RetakeApplicationId = testAppointment.RetakeTestApplicationID ?? 0;
            }
            else
            {
                resposne.RetakeTestApplication = null;
            }

            return Success(resposne);
        }
    }
}
