using DVLD.Domain.Common.Enums;

namespace DVLD.Application.Features.TestAppointment.Commands.AddTestAppointmentCommand
{
    public class AddTestAppointmentCommandHandler
        (ILocalApplicationRepository localApplicationRepository, IApplicationRepository applicationRepository,
        IApplicationTypeRepository applicationTypeRepository, ITestTypeRepository testTypeRepository, ITestAppointmentRepository testAppointmentRepository)
        : ResponseHandler, IRequestHandler<AddTestAppointmentCommand, Response<int?>>
    {
        public async Task<Response<int?>> Handle(AddTestAppointmentCommand request, CancellationToken cancellationToken)
        {
            AllEntities.LocalApplication? localApplication = await localApplicationRepository.GetByIdAsync(request.LocalApplicationId);

            if (localApplication is null)
            {
                return NotFound<int?>("Local application not found");
            }

            int? RetakeTestApplicationId = null;
            if (await localApplicationRepository.DoesAttendTestType(request.LocalApplicationId, (int)request.TestTypeId))
            {
                RetakeTestApplicationId = await HandleRetakeApplication(localApplication, request.CreatedUserId);
                if (RetakeTestApplicationId is null)
                {
                    return Fail<int?>(null, "Faild to Create retake test application");
                }
            }

            AllEntities.TestType? TestType = await testTypeRepository.GetByIdAsync((int)request.TestTypeId);

            if (TestType == null)
            {
                return Fail<int?>(null, "Error retrieving necessary information for adding apointment.");
            }
            AllEntities.TestAppointment TestAppointment = new();

            TestAppointment.TestTypeID = request.TestTypeId;
            TestAppointment.LocalApplicationID = request.LocalApplicationId;
            TestAppointment.AppointmentDate = request.AppointmentDate;
            TestAppointment.CreatedByUserID = request.CreatedUserId;

            TestAppointment.RetakeTestApplicationID = RetakeTestApplicationId;
            TestAppointment.PaidFees = TestType.TestTypeFees;

            if (!await testAppointmentRepository.SaveAsync(TestAppointment))
            {
                return Fail<int?>(null, "Error adding test appointment");
            }

            return Created<int?>(TestAppointment.Id);
        }

        private async Task<int?> HandleRetakeApplication(AllEntities.LocalApplication localApplication, int CreatedUserId)
        {
            Task<AllEntities.ApplicationType?> TaskApplicationType = applicationTypeRepository.GetByIdAsync((int)enApplicationType.RetakeTest);
            Task<AllEntities.Application?> ApplicationTask = applicationRepository.GetByIdAsync(localApplication.ApplicationId);

            await Task.WhenAll(TaskApplicationType, ApplicationTask);
            if (TaskApplicationType.Result == null || ApplicationTask.Result == null)
            {
                return null;
            }

            AllEntities.ApplicationType ApplicationType = TaskApplicationType.Result;
            localApplication.ApplicationInfo = ApplicationTask.Result;

            AllEntities.Application Application = new();
            Application.ApplicantPersonID = localApplication.ApplicationInfo.ApplicantPersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = enApplicationType.RetakeTest;
            Application.ApplicationStatusID = enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = ApplicationType.ApplicationFees;
            Application.CreatedByUserID = CreatedUserId;

            if (!await applicationRepository.SaveAsync(Application))
            {
                return null;
            }
            return Application.Id;
        }
    }
}
