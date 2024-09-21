using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataLayerCore.Driver
{
    public class clsDriverData
    {
        public static async Task<DriverDTO?> GetDriverInfoByDriverID(int DriverID)
        {
            DriverDTO? driverDTO = null;
            await DataSendhandler.handle("SP_FindDriverById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        driverDTO = Reader.MapTo<DriverDTO>();
                    }
                }
            });

            return driverDTO;
        }

        public static async Task<DriverDTO?> GetDriverInfoByPersonID(int PersonID)
        {
            DriverDTO? driverDTO = null;

            await DataSendhandler.handle("SP_FindDriverByPersonID", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        driverDTO = Reader.MapTo<DriverDTO>();
                    }
                }
            });

            return driverDTO;
        }

        public static async Task<int?> AddNewDriver(DriverForCreateDTO NewDriver)
        {
            int? DriverID = null;
            await DataSendhandler.handle("SP_AddNewDriver", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@PersonID", NewDriver.PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", NewDriver.CreatedByUserID);
                Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                }
            });

            return DriverID;
        }

        public static async Task<bool> UpdateDriver(int DriverID, DriverForUpdateDTO NewDriver)
        {

            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateDriver", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@DriverID", DriverID);
                Command.Parameters.AddWithValue("@PersonID", NewDriver.PersonID);
                Command.Parameters.AddWithValue("@CreatedByUserID", NewDriver.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        public static async Task<List<DriverPrefDTO>> GetAllDrivers()
        {
            var list = new List<DriverPrefDTO>();
            await DataSendhandler.handle("SP_GetAllDrivers", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<DriverPrefDTO>());
                    }
                }
            });

            return list;

        }
    }
}
