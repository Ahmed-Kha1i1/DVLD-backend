using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DataLayerCore.LicenseClass
{
    public class clsLicenseClassData
    {
        public static async Task<LicenseClassDTO?> GetLicenseClassInfoByID(int LicenseClassID)
        {
            LicenseClassDTO? licenseClassDTO = null;
            await DataSendhandler.handle("SP_FindLicenseClassById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        licenseClassDTO = Reader.MapTo<LicenseClassDTO>();
                    }
                }
            });

            return licenseClassDTO;
        }

        public static async Task<LicenseClassDTO?> GetLicenseClassInfoByClassName(string ClassName)
        {
            LicenseClassDTO? licenseClassDTO = null;
            await DataSendhandler.handle("SP_FindLicenseClassInfoByClassName", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ClassName", ClassName);
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        licenseClassDTO = Reader.MapTo<LicenseClassDTO>();
                    }
                }
            });


            return licenseClassDTO;
        }

        public static async Task<int?> AddNewLicenseClass(LicenseClassForCreateDTO LicenseClass)
        {
            int? LicenseClassID = null;
            await DataSendhandler.handle("SP_AddNewLicenseClass", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ClassName", LicenseClass.ClassName);
                Command.Parameters.AddWithValue("@ClassDescription", LicenseClass.ClassDescription);
                Command.Parameters.AddWithValue("@MinimumAllowedAge", LicenseClass.MinimumAllowedAge);
                Command.Parameters.AddWithValue("@DefaultValidityLength", LicenseClass.DefaultValidityLength);
                Command.Parameters.AddWithValue("@ClassFees", LicenseClass.ClassFees);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
                }
            });

            return LicenseClassID;

        }

        public static async Task<bool> UpdateLicenseClass(int LicenseClassID, LicenseClassForUpdateDTO LicenseClass)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateLicenseClass", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                Command.Parameters.AddWithValue("@ClassName", LicenseClass.ClassName);
                Command.Parameters.AddWithValue("@ClassDescription", LicenseClass.ClassDescription);
                Command.Parameters.AddWithValue("@MinimumAllowedAge", LicenseClass.MinimumAllowedAge);
                Command.Parameters.AddWithValue("@DefaultValidityLength", LicenseClass.DefaultValidityLength);
                Command.Parameters.AddWithValue("@ClassFees", LicenseClass.ClassFees);


                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<List<LicenseClassDTO>> GetAllLicenseClasses()
        {
            var list = new List<LicenseClassDTO>();
            await DataSendhandler.handle("SP_GetAllLicenseClasses", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<LicenseClassDTO>());
                    }
                }
            });

            return list;

        }
    }
}
