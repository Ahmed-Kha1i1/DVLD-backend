using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using static System.Net.Mime.MediaTypeNames;


namespace DataLayerCore.InternationalLicense
{

    public class clsInternationalLicenseData
    {
        public static async Task<InternationalLicenseDTO?> GetInternationalLicense(int InternationalLicenseID)
        {
            InternationalLicenseDTO? internationalLicenseDTO = null;
            await DataSendhandler.handle("SP_FindInternationalLicensById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        internationalLicenseDTO = Reader.MapTo<InternationalLicenseDTO>();
                    }
                }
            });

            return internationalLicenseDTO;

        }

        public static async Task<int?> GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int? InternationalLicenseID = null;
            await DataSendhandler.handle("SP_GetActiveInternationalLicenseIDByDriverID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            });


            return InternationalLicenseID;
        }

        public static async Task<int?> AddNewInternationalLicense(InternationalLicenseForCreateDTO InternationalLicense)
        {
            int? InternationalLicenseID = null;
            await DataSendhandler.handle("SP_AddNewInternationalLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", InternationalLicense.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", InternationalLicense.DriverID);
                Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", InternationalLicense.IssuedUsingLocalLicenseID);
                Command.Parameters.AddWithValue("@IssueDate", InternationalLicense.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", InternationalLicense.ExpirationDate);
                Command.Parameters.AddWithValue("@IsActive", InternationalLicense.IsActive);
                Command.Parameters.AddWithValue("@CreatedByUserID", InternationalLicense.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            });

            return InternationalLicenseID;


        }

        public static async Task<bool> UpdateInternationalLicense(int InternationalLicenseID, InternationalLicenseForUpdateDTO InternationalLicense)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateInternationalLicens", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                Command.Parameters.AddWithValue("@ApplicationID", InternationalLicense.ApplicationID);
                Command.Parameters.AddWithValue("@DriverID", InternationalLicense.DriverID);
                Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", InternationalLicense.IssuedUsingLocalLicenseID);
                Command.Parameters.AddWithValue("@IssueDate", InternationalLicense.IssueDate);
                Command.Parameters.AddWithValue("@ExpirationDate", InternationalLicense.ExpirationDate);
                Command.Parameters.AddWithValue("@IsActive", InternationalLicense.IsActive);
                Command.Parameters.AddWithValue("@CreatedByUserID", InternationalLicense.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        public static async Task<List<InternationalLicenseDTO>> GetInternationalLicenses()
        {
            var list = new List<InternationalLicenseDTO>();
            await DataSendhandler.handle("SP_GetAllInternationalLicenses", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<InternationalLicenseDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<List<InternationalLicenseDTO>> GetInternationalLicenses(int DriverID)
        {
            var list = new List<InternationalLicenseDTO>();
            await DataSendhandler.handle("SP_GetAllInternationalLicensesByDriverId", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<InternationalLicenseDTO>());
                    }
                }
            });

            return list;
        }

    }
}
