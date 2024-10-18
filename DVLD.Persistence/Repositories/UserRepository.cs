using AutoMapper;
using DVLD.Application.Contracts.Persistence;
using DVLD.Application.Features.Users;
using DVLD.Domain.Entities;
using DVLD.Persistence.Handlers;
using DVLD.Persistence.Repositories.Base;

namespace DVLD.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        DataSendhandler _dataSendhandler;
        public UserRepository(DataSendhandler dataSendhandler, IMapper mapper) : base(dataSendhandler, mapper)
        {
            _dataSendhandler = dataSendhandler;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await GetItemAsync<User>("SP_FindUserById", id, "UserID");
        }

        public async Task<Person?> GetPerson(int userId)
        {
            return await GetItemAsync<Person?>("SP_FindPersondByUserId", userId, "UserID");
        }

        public async Task<IReadOnlyList<UserOverviewDTO>> ListUsersOverviewAsync()
        {
            return await ListAllAsync<UserOverviewDTO>("SP_GetAllUsers");
        }

        public async Task<bool> UpdatePassword(int userId, string NewPassword)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdatePassword", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", userId);
                Command.Parameters.AddWithValue("@NewPassword", NewPassword);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        protected override async Task<int?> AddAsync(User user)
        {
            int? UserID = null;
            await _dataSendhandler.Handle("SP_AddNewUser", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", user.PersonID);
                Command.Parameters.AddWithValue("@UserName", user.UserName);
                Command.Parameters.AddWithValue("@Password", user.Password);
                Command.Parameters.AddWithValue("@IsActive", user.IsActive);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            });


            return UserID;
        }

        protected override async Task<bool> UpdateAsync(User user)
        {
            int RowAffected = 0;
            await _dataSendhandler.Handle("SP_UpdateUser", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", user.Id);
                Command.Parameters.AddWithValue("@PersonID", user.PersonID);
                Command.Parameters.AddWithValue("@UserName", user.UserName);
                Command.Parameters.AddWithValue("@IsActive", user.IsActive);

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            return await DeleteAsync("SP_DeleteUserById", id, "UserID");
        }

        public async Task<bool> IsUserActive(int userId)
        {
            return await CheckConditionAsync("SP_IsUserActive", userId, "UserID");
        }

        public async Task<bool> IsUserExistByPersonId(int personId)
        {
            return await CheckConditionAsync("SP_IsUserExistsByPersonId", personId, "PersonID");
        }

        public async Task<bool> IsUserExistByUserId(int userId)
        {
            return await CheckConditionAsync("SP_IsUserExistsByid", userId, "UserID");
        }

        public async Task<bool> IsUserExistByUserName(string userName)
        {
            return await CheckConditionAsync("SP_IsUserExistsByUsername", userName, "UserName");
        }

        public async Task<bool> IsUsernameUnique(string Username, int? userId)
        {
            return await IsUnique("SP_IsUsernameUnique", Username, "Username", userId);
        }

        public async Task<bool> IsPersonIdUnique(int PersonId, int? userId = null)
        {
            return await IsUnique("SP_IsPersonIdUnique", PersonId, "PersonId", userId);
        }
    }
}
