using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataLayerCore.LocalDrivingLicenseApplication
{
    public class clsLocalDrivingLicenseApplicationData
    {
        public static async Task<LocalDrivingLicenseApplicationDTO?> GetLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            LocalDrivingLicenseApplicationDTO? localDrivingLicenseApplicationDTO = null;
            await DataSendhandler.handle("SP_FindLocalDrivingLicenseApplicationById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        localDrivingLicenseApplicationDTO = Reader.MapTo<LocalDrivingLicenseApplicationDTO>();
                    }
                }
            });

            return localDrivingLicenseApplicationDTO;
        }

        public static async Task<LocalDrivingLicenseApplicationDTO?> GetLocalDrivingLicenseApplicationByApplicationID(int ApplicationID)
        {
            LocalDrivingLicenseApplicationDTO? localDrivingLicenseApplicationDTO = null;
            await DataSendhandler.handle("SP_FindLocalDrivingLicenseApplicationByApplicationID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        localDrivingLicenseApplicationDTO = Reader.MapTo<LocalDrivingLicenseApplicationDTO>();
                    }
                }
            });


            return localDrivingLicenseApplicationDTO;
        }

        public static async Task<int?> AddNewLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationForCreateDTO LocalDrivingLicenseApplication)
        {
            int? LocalDrivingLicenseApplicationID = null;
            await DataSendhandler.handle("SP_AddNewLocalDrivingLicenseApplication", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", LocalDrivingLicenseApplication.ApplicationID);
                Command.Parameters.AddWithValue("@LicenseClassID", LocalDrivingLicenseApplication.LicenseClassID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            });

            return LocalDrivingLicenseApplicationID;

        }

        public static async Task<bool> UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, LocalDrivingLicenseApplicationForUpdateDTO LocalDrivingLicenseApplication)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateLocalDrivingLicenseApplication", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@ApplicationID", LocalDrivingLicenseApplication.ApplicationID);
                Command.Parameters.AddWithValue("@LicenseClassID", LocalDrivingLicenseApplication.LicenseClassID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;

        }

        public static async Task<bool> DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_DeleteLocalDrivingLicenseApplicationById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<List<LocalDrivigFullData>> GetLocalDrivingLicenseApplications()
        {
            var list = new List<LocalDrivigFullData>();
            await DataSendhandler.handle("SP_GetAllLocalDrivingLicenseApplications", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<LocalDrivigFullData>());
                    }
                }
            });

            return list;

        }

        public static async Task<bool> DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            await DataSendhandler.handle("SP_DoesPassTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            });

            return Result;
        }
        public static async Task<bool> DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            await DataSendhandler.handle("SP_DoesAttendTestType", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    Result = true;
                }
            });


            return Result;
        }
        public static async Task<bool> IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;
            await DataSendhandler.handle("SP_IsThereAnActiveScheduledTest", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    Result = true;
                }
            });

            return Result;
        }
        public static async Task<byte> TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;
            await DataSendhandler.handle("SP_getTotalTrialsPerTest", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && byte.TryParse(result.ToString(), out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            });


            return TotalTrialsPerTest;
        }



    }
}
