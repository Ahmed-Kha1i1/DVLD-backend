using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.People.Common.Models;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IPersonRepository : IAsyncRepository<Person>, IAsyncListOverviewRepository<PersonOverviewDTO>
    {
        Task<Person?> GetAsync(string NationalNo);
        Task<bool> IsNationalNoUnique(string NationalNo, int? Id = null);
        Task<bool> IsEmailUnique(string Email, int? Id = null);
        Task<bool> IsPhoneUnique(string Phone, int? Id = null);
        Task<bool> IsPersonExists(int PersonId);
        Task<int?> GetDriverId(int PersonId);
    }
}
