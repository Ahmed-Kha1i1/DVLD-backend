using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace DataLayerCore.DetainedLicense
{
    public class clsDetainedLicenseData
    {
        public static async Task<DetainedLicenseDTO?> GetDetainedLicenseInfoByID(int DetainID)
        {
            DetainedLicenseDTO? detainedLicenseDTO = null;
            await DataSendhandler.handle("SP_FindDetainedLicensById", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@DetainID", DetainID);

                using SqlDataReader Reader = await Command.ExecuteReaderAsync();
                if (Reader.Read())
                {
                    detainedLicenseDTO = Reader.MapTo<DetainedLicenseDTO>();
                }

            });

            return detainedLicenseDTO;

        }

        public static async Task<DetainedLicenseDTO?> GetDetainedLicenseInfoByLicenseID(int LicenseID)
        {
            DetainedLicenseDTO? detainedLicenseDTO = null;
            await DataSendhandler.handle("SP_GetDetainedLicenseInfoByLicenseID", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                using SqlDataReader Reader = await Command.ExecuteReaderAsync();
                if (Reader.Read())
                {
                    detainedLicenseDTO = Reader.MapTo<DetainedLicenseDTO>();
                }
            });

            return detainedLicenseDTO;

        }

        public static async Task<int?> AddNewDetainedLicense(DetainedLicenseForCreateDTO NewDetainedLicense)
        {
            int? DetainID = null;
            await DataSendhandler.handle("SP_AddNewDetainedLicens", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@LicenseID", NewDetainedLicense.LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", NewDetainedLicense.DetainDate);
                Command.Parameters.AddWithValue("@FineFees", NewDetainedLicense.FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", NewDetainedLicense.CreatedByUserID);

                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    DetainID = ID;
                }
            });

            return DetainID;

        }

        public static async Task<bool> UpdateDetainedLicense(int DetainID, DetainedLicenseForUpdateDTO NewDetainedLicense)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateDetainedLicens", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                Command.Parameters.AddWithValue("@LicenseID", NewDetainedLicense.LicenseID);
                Command.Parameters.AddWithValue("@DetainDate", NewDetainedLicense.DetainDate);
                Command.Parameters.AddWithValue("@FineFees", NewDetainedLicense.FineFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", NewDetainedLicense.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        public static async Task<List<DetainedLicenseFullDTO>> GetAllDetainedLicenses()
        {
            var list = new List<DetainedLicenseFullDTO>();
            await DataSendhandler.handle("SP_GetAllDetainedLicenses", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<DetainedLicenseFullDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<bool> ReleaseDetainedLicense(ReleaseDetainedLicenseDTO releaseDetainedLicenseDTO)
        {

            int rowsAffected = 0;
            await DataSendhandler.handle("SP_ReleaseDetainedLicense", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@DetainID", releaseDetainedLicenseDTO.DetainID);
                Command.Parameters.AddWithValue("@ReleasedByUserID", releaseDetainedLicenseDTO.ReleasedByUserID);
                Command.Parameters.AddWithValue("@ReleaseApplicationID", releaseDetainedLicenseDTO.ReleaseApplicationID);
                Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                rowsAffected = await Command.ExecuteNonQueryAsync();

            });

            return rowsAffected > 0;
        }

        public static async Task<bool> IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            await DataSendhandler.handle("SP_IsLicenseDetained", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            });
            return IsDetained;
        }
    }
}
