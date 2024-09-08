using DataLayerCore;
using DataLayerCore.Datahandler;
using DataLayerCore.TestType;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataLayerCore.TestAppointment
{
    public class clsTestAppointmentData
    {
        public static async Task<TestAppointmentDTO?> FindTestAppointment(int TestAppointmentID)
        {
            TestAppointmentDTO? testAppointmentDTO = null;
            await DataSendhandler.handle("SP_FindTestAppointmentById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        testAppointmentDTO = Reader.MapTo<TestAppointmentDTO>();
                    }
                }
            });

            return testAppointmentDTO;

        }

        public static async Task<TestAppointmentDTO?> GetLastTestAppointment(int LocalDrivingLicenseApplicationID, enTestType TestTypeID)
        {
            TestAppointmentDTO? testAppointmentDTO = null;
            await DataSendhandler.handle("SP_FindLastTestAppointment", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        testAppointmentDTO = Reader.MapTo<TestAppointmentDTO>();
                    }
                }
            });

            return testAppointmentDTO;

        }

        public static async Task<int?> AddNewTestAppointment(TestAppointmentForCreateDTO TestAppointment)
        {
            int? TestAppointmentID = null;
            await DataSendhandler.handle("SP_AddNewTestAppointment", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", TestAppointment.TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", TestAppointment.LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@AppointmentDate", TestAppointment.AppointmentDate);
                Command.Parameters.AddWithValue("@PaidFees", TestAppointment.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", TestAppointment.CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", TestAppointment.IsLocked);
                if (TestAppointment.RetakeTestApplicationID == null)
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", TestAppointment.RetakeTestApplicationID);
                }
                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                }
            });

            return TestAppointmentID;
        }

        public static async Task<bool> UpdateTestAppointment(int TestAppointmentID, TestAppointmentForUpdateDTO TestAppointment)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateTestAppointment", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Command.Parameters.AddWithValue("@TestTypeID", TestAppointment.TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", TestAppointment.LocalDrivingLicenseApplicationID);
                Command.Parameters.AddWithValue("@AppointmentDate", TestAppointment.AppointmentDate);
                Command.Parameters.AddWithValue("@PaidFees", TestAppointment.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", TestAppointment.CreatedByUserID);
                Command.Parameters.AddWithValue("@IsLocked", TestAppointment.IsLocked);

                if (TestAppointment.RetakeTestApplicationID == null)
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", TestAppointment.RetakeTestApplicationID);
                }

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<bool> DeleteTestAppointment(int TestAppointmentID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_DeleteTestAppointmentById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });


            return RowAffected > 0;
        }

        public static async Task<List<TestAppointmentFullDTO>> GetAllTestAppointments(enTestType TestTypeID, int LDLAppID)
        {
            var list = new List<TestAppointmentFullDTO>();
            await DataSendhandler.handle("SP_GetAllTestAppointments", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<TestAppointmentFullDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<List<TestAppointmentPrefDTO>> GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            var list = new List<TestAppointmentPrefDTO>();
            await DataSendhandler.handle("SP_GetApplicationTestAppointmentsPerTestType", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<TestAppointmentPrefDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<bool> LockAppointment(int TestAppointmentID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_LockAppointment", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                Connection.Open();
                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<int?> GetTestID(int TestAppointmentID)
        {
            int? TestID = null;
            await DataSendhandler.handle("SP_GetTestID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    TestID = ptCount;
                }
            });

            return TestID;


        }
    }
}
