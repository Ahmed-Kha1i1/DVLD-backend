using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.People;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IPersonRepository : IAsyncRepository<Person>
    {
        Task<Person?> GetAsync(string NationalNo);
        Task<IReadOnlyList<PersonOverviewDTO>> ListPeopleOverviewAsync();
        Task<bool> IsNationalNoUnique(string NationalNo, int? Id = null);
        Task<bool> IsEmailUnique(string Email, int? Id = null);
        Task<bool> IsPhoneUnique(string Phone, int? Id = null);
        Task<bool> IsPersonExists(int Id);
    }
}
