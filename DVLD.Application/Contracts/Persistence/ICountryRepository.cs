using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.Countries;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface ICountryRepository : IAsyncGetRepository<Country>
    {
        Task<Country?> GetByNameAsync(string CountryName);
        Task<IReadOnlyList<CountryDTO>> ListAsync();
    }
}
