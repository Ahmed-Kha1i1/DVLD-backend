using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.TestAppointment.Common.Models;
using DVLD.Domain.Common.Enums;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ITestAppointmentRepository : IAsyncModificationRepository<TestAppointment>, IAsyncGetRepository<TestAppointment>
    {
        Task<IReadOnlyList<TestAppointmentOverviewDTO>> ListPerTestTypeAsync(int LocalApplciationId, enTestType enTestType);
        Task<bool> LockAppointment(int TestAppointmentID);

    }

}
