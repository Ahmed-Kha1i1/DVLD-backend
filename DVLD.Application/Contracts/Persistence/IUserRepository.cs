using DVLD.Application.Contracts.Persistence.Base;
using DVLD.Application.Features.Users;
using DVLD.Domain.Entities;

namespace DVLD.Application.Contracts.Persistence
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<Person?> GetPerson(int userId);
        Task<IReadOnlyList<UserOverviewDTO>> ListUsersOverviewAsync();
        Task<bool> UpdatePassword(int userId, string NewPassword);
        Task<bool> IsUserExistByUserId(int userId);
        Task<bool> IsUserExistByPersonId(int personId);
        Task<bool> IsUserExistByUserName(string userName);
        Task<bool> IsUserActive(int userId);
        Task<bool> IsUsernameUnique(string Username, int? userId = null);
        Task<bool> IsPersonIdUnique(int PersonId, int? userId = null);
        Task<bool> IsPasswordValid(int UserId, string Password);
    }
}
