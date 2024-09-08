using DataLayerCore;
using DataLayerCore.Datahandler;
using DataLayerCore.TestType;
using Microsoft.Data.SqlClient;
using System.Data;
using static Azure.Core.HttpHeader;


namespace DataLayerCore.Test
{
    public partial class clsTestData
    {
        public static async Task<TestDTO?> GetTest(int TestID)
        {
            TestDTO? testDTO = null;
            await DataSendhandler.handle("SP_FindTestById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestID", TestID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        testDTO = Reader.MapTo<TestDTO>();
                    }
                }
            });

            return testDTO;
        }
        public static async Task<TestDTO?> GetLastTest(enTestType TestTypeID, LastTestDTO lastTestDTO)
        {
            TestDTO? testDTO = null;
            await DataSendhandler.handle("SP_GetLastTest", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@PersonID", lastTestDTO.PersonID);
                Command.Parameters.AddWithValue("@LicenseClassID", lastTestDTO.LicenseClassID);
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        testDTO = Reader.MapTo<TestDTO>();
                    }
                }
            });

            return testDTO;


        }

        public static async Task<int?> AddNewTest(TestForCreateDTO Test)
        {
            int? TestID = null;
            await DataSendhandler.handle("SP_AddNewTest", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestAppointmentID", Test.TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", Test.TestResult);
                if (string.IsNullOrWhiteSpace(Test.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", Test.Notes);
                }
                Command.Parameters.AddWithValue("@CreatedByUserID", Test.CreatedByUserID);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            });

            return TestID;
        }

        public static async Task<bool> UpdateTest(int TestID, TestForUpdateDTO Test)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateTest", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestID", TestID);
                Command.Parameters.AddWithValue("@TestAppointmentID", Test.TestAppointmentID);
                Command.Parameters.AddWithValue("@TestResult", Test.TestResult);
                if (string.IsNullOrWhiteSpace(Test.Notes))
                {
                    Command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    Command.Parameters.AddWithValue("@Notes", Test.Notes);
                }
                Command.Parameters.AddWithValue("@CreatedByUserID", Test.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });


            return RowAffected > 0;
        }

        public static async Task<List<TestDTO>> GetTests()
        {
            var list = new List<TestDTO>();
            await DataSendhandler.handle("SP_GetAllTests", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<TestDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<int> GetTestpassCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;
            await DataSendhandler.handle("SP_GetTestpassCount", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
                }
            });

            return PassedTestCount;


        }

    }
}
