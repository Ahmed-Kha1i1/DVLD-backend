using DataLayerCore.Datahandler;
using DataLayerCore.Person;
using Microsoft.Data.SqlClient;
namespace DataLayerCore.User
{
    public class clsUserData
    {
        public static async Task<UserDTO?> GetUser(int UserID)
        {
            UserDTO? userDTO = null;
            await DataSendhandler.handle("SP_FindUserById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@UserID", UserID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        userDTO = Reader.MapTo<UserDTO>();
                    }
                }
            });

            return userDTO;
        }

        public static async Task<PersonDTO?> GetPerson(int userId)
        {
            PersonDTO? personDTO = null;
            await DataSendhandler.handle("SP_FindPersondByUserId", async (Connection, Command) =>
            {

                Command.Parameters.AddWithValue("@UserID", userId);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        personDTO = Reader.MapTo<PersonDTO>();
                    }
                }

            });

            return personDTO;

        }

        public static async Task<UserDTO?> GetUserByPersonID(int PersonID)
        {
            UserDTO? userDTO = null;
            await DataSendhandler.handle("SP_FindUserByPersonID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        userDTO = Reader.MapTo<UserDTO>();
                    }
                }
            });

            return userDTO;
        }

        public static async Task<UserDTO?> GetUserInfoByCredentials(string Username, string Password)
        {
            UserDTO? userDTO = null;
            await DataSendhandler.handle("SP_GetUserInfoByCredentials", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@Username", Username);
                Command.Parameters.AddWithValue("@Password", Username);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        userDTO = Reader.MapTo<UserDTO>();
                    }
                }
            });

            return userDTO;
        }

        public static async Task<int?> AddNewUser(UserFordatabaseDTO User)
        {
            int? UserID = null;
            await DataSendhandler.handle("SP_AddNewUser", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", User.PersonID);
                Command.Parameters.AddWithValue("@UserName", User.UserName);
                Command.Parameters.AddWithValue("@Password", User.Password);
                Command.Parameters.AddWithValue("@IsActive", User.IsActive);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            });


            return UserID;

        }

        public static async Task<bool> UpdateUser(int UserID, UserDTO User)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateUser", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);
                Command.Parameters.AddWithValue("@PersonID", User.PersonID);
                Command.Parameters.AddWithValue("@UserName", User.UserName);
                Command.Parameters.AddWithValue("@IsActive", User.IsActive);

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        public static async Task<bool> DeleteUser(int UserID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_DeleteUserById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<List<UserPrefDTO>> GetAllUsers()
        {
            var list = new List<UserPrefDTO>();
            await DataSendhandler.handle("SP_GetAllUsers", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<UserPrefDTO>());
                    }
                }
            });

            return list;

        }

        public static async Task<bool> IsUserExistByUserID(int UserID)
        {

            bool IsFound = false;
            await DataSendhandler.handle("SP_IsUserExistsByid", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null;
            });

            return IsFound;


        }

        public static async Task<bool> IsUserExistByPersonID(int PersonID)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsUserExistsByPersonId", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null && (int)Result == 1;
            });

            return IsFound;
        }

        public static async Task<bool> IsUserExistByUserName(string Username)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsUserExistsByUsername", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserName", Username);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null;
            });

            return IsFound;
        }

        public static async Task<bool> IsUserActive(int UserID)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsUserActive", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);
                object? Result = await Command.ExecuteScalarAsync();

                IsFound = Result is not null;
            });


            return IsFound;
        }

        public static async Task<bool> IsUsernameUnique(string Username, int? Id)
        {
            bool IsUnique = false;
            await DataSendhandler.handle("SP_IsUsernameUnique", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@Username", Username);
                if (Id is not null)
                    Command.Parameters.AddWithValue("@Id", Id);
                object? Result = await Command.ExecuteScalarAsync();

                IsUnique = Result is not null && (int)Result == 1;
            });

            return IsUnique;
        }

        public static async Task<bool> UpdatePassword(int UserId, UpdatePasswordDTO UpdatePasswordDTO)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdatePassword", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserId);
                Command.Parameters.AddWithValue("@NewPassword", UpdatePasswordDTO.Password);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

    }
}
