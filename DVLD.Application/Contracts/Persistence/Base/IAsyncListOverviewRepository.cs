namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncListOverviewRepository<T>
    {
        Task<IReadOnlyList<T>> ListOverviewAsync();
    }
}
