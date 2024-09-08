using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.Data;
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

        public static async Task<int?> AddNewUser(UserForCreateDTO User)
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

        public static async Task<bool> UpdateUser(int UserID, UserForUpdateDTO User)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateUser", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);
                Command.Parameters.AddWithValue("@PersonID", User.PersonID);
                Command.Parameters.AddWithValue("@UserName", User.UserName);
                Command.Parameters.AddWithValue("@Password", User.Password);
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

        public static async Task<List<UserFullDTO>> GetAllUsers()
        {
            var list = new List<UserFullDTO>();
            await DataSendhandler.handle("SP_GetAllUsers", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<UserFullDTO>());
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

                IsFound = Result is not null;
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

        public static async Task<bool> UpdatePassword(int UserID, string NewPassword)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdatePassword", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@UserID", UserID);
                Command.Parameters.AddWithValue("@NewPassword", NewPassword);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

    }
}
