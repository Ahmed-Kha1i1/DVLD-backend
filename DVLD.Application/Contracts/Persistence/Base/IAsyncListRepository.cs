
namespace DVLD.Application.Contracts.Persistence.Base
{
    public interface IAsyncListRepository<T>
    {
        Task<IReadOnlyList<T>> ListAllAsync();
    }

}