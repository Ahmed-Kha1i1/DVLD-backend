using DVLD.Application.Contracts.Persistence.Base;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ITestRepository : IAsyncModificationRepository<AllEntities.Test>, IAsyncGetRepository<AllEntities.Test>
    {
        Task<bool> PassedAllTests(int LocalApplicationID);
        Task<int?> GetTestpassCount(int LocalApplicationID);
    }
}
