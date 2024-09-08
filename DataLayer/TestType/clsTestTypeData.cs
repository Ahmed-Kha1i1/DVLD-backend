using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DataLayerCore.TestType
{

    public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3, None = -1 };
    public class clsTestTypeData
    {
        public static async Task<TestTypeDTO?> GetTestType(enTestType TestTypeID)
        {
            TestTypeDTO? testTypeDTO = null;
            await DataSendhandler.handle("SP_FindTestTypeById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        testTypeDTO = Reader.MapTo<TestTypeDTO>();
                    }
                }
            });

            return testTypeDTO;
        }

        public static async Task<bool> UpdateTestType(int TestTypeID, TestTypeForUpdateDTO TestType)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateTestType", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                Command.Parameters.AddWithValue("@TestTypeTitle", TestType.TestTypeTitle);
                Command.Parameters.AddWithValue("@TestTypeDescription", TestType.TestTypeDescription);
                Command.Parameters.AddWithValue("@TestTypeFees", TestType.TestTypeFees);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<int> AddNewTestType(TestTypeForCreateDTO TestType)
        {
            int TestTypeID = -1;
            await DataSendhandler.handle("SP_AddNewTestType", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@TestTypeTitle", TestType.TestTypeTitle);
                Command.Parameters.AddWithValue("@TestTypeDescription", TestType.TestTypeDescription);
                Command.Parameters.AddWithValue("@TestTypeFees", TestType.TestTypeFees);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                }
            });


            return TestTypeID;
        }

        public static async Task<List<TestTypeDTO>> GetTestTypes()
        {
            var list = new List<TestTypeDTO>();
            await DataSendhandler.handle("SP_GetAllTestTypes", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<TestTypeDTO>());
                    }
                }
            });


            return list;
        }

    }
}
