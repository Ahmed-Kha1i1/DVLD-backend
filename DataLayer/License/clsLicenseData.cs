using DataLayerCore;
using DataLayerCore.Datahandler;
using DataLayerCore.Driver;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace DataLayerCore.License
{
    public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementDamaged = 3, ReplacementLost = 4 }
    public class clsLicenseData
    {
        public static async Task<LicenseInfoDTO?> GetLicenseInfoByLicenseID(int LicenseID)
        {
            LicenseInfoDTO? licenseInfoDTO = null;
            await DataSendhandler.handle("SP_FindLicensById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        licenseInfoDTO = Reader.MapTo<LicenseInfoDTO>();
                    }
                }
            });


            return licenseInfoDTO;
        }

        public static async Task<LicenseInfoDTO?> GetLicenseInfoByApplicationID(int ApplicationID)
        {
            LicenseInfoDTO? licenseInfoDTO = null;
            await DataSendhandler.handle("SP_FindLicensByApplicationID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@@ApplicationID", @ApplicationID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        licenseInfoDTO = Reader.MapTo<LicenseInfoDTO>();
                    }
                }
            });


            return licenseInfoDTO;
        }

        public static async Task<int?> AddNewLicense(LicenseInfoForCreateDTO LicenseInfo)
        {

            int? LicenseID = null;
            await DataSendhandler.handle("SP_AddNewLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", LicenseInfo.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", LicenseInfo.DriverID);
                Command.Parameters.AddWithValue("@LicenseClass", LicenseInfo.LicenseClass);
                Command.Parameters.AddWithValue("@IssueDate", LicenseInfo.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", LicenseInfo.ExpirationDate);
                if (string.IsNullOrWhiteSpace(LicenseInfo.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", LicenseInfo.Notes);
                }
                Command.Parameters.AddWithValue("@PaidFees", LicenseInfo.PaidFees);
                Command.Parameters.AddWithValue("@IsActive", LicenseInfo.IsActive);
                Command.Parameters.AddWithValue("@IssueReason", LicenseInfo.IssueReason);
                Command.Parameters.AddWithValue("@CreatedByUserID", LicenseInfo.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            });

            return LicenseID;

        }

        public static async Task<bool> UpdateLicense(int LicenseID, LicenseInfoForUpdateDTO LicenseInfo)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                Command.Parameters.AddWithValue("@ApplicationID", LicenseInfo.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", LicenseInfo.DriverID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseInfo.LicenseClass);
                Command.Parameters.AddWithValue("@IssueDate", LicenseInfo.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", LicenseInfo.ExpirationDate);
                if (string.IsNullOrWhiteSpace(LicenseInfo.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", LicenseInfo.Notes);
                }
                Command.Parameters.AddWithValue("@PaidFees", LicenseInfo.PaidFees);
                Command.Parameters.AddWithValue("@IsActive", LicenseInfo.IsActive);
                Command.Parameters.AddWithValue("@IssueReason", LicenseInfo.IssueReason);
                Command.Parameters.AddWithValue("@CreatedByUserID", LicenseInfo.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        public static async Task<List<LicenseInfoDTO>> GetLicenses()
        {
            var list = new List<LicenseInfoDTO>();
            await DataSendhandler.handle("SP_GetAllLicenses", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<LicenseInfoDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<List<DriverLicenseDTO>> GetDriverLicenses(int DriverID)
        {
            var list = new List<DriverLicenseDTO>();
            await DataSendhandler.handle("SP_FindDriverLicenses", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<DriverLicenseDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<int?> GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int? LicenseID = null;
            await DataSendhandler.handle("SP_FindActiveLicenseIDByPersonID", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);


                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;

                }
            });

            return LicenseID;
        }

        public static async Task<bool> DeactivateLicense(int LicenseID)
        {

            int rowsAffected = 0;
            await DataSendhandler.handle("SP_DeactivateLicense", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                rowsAffected = await Command.ExecuteNonQueryAsync();
            });

            return rowsAffected > 0;
        }

    }
}
