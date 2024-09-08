using DataLayerCore;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;
using System.Data;


namespace DataLayerCore.ApplicationType
{
    public enum enApplicationType
    {
        NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
        ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7,
        None = -1
    };

    public class clsApplicationTypeData
    {
        public static async Task<ApplicationTypeDTO?> GetApplicationType(enApplicationType ApplicationTypeID)
        {
            ApplicationTypeDTO? applicationTypeDTO = null;
            await DataSendhandler.handle("SP_FindApplicationTypeById", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {

                    if (Reader.Read())
                    {
                        applicationTypeDTO = Reader.MapTo<ApplicationTypeDTO>();
                    }
                }

            });

            return applicationTypeDTO;
        }

        public static async Task<List<ApplicationTypeDTO>> GetApplicationTypes()
        {
            var list = new List<ApplicationTypeDTO>();
            await DataSendhandler.handle("SP_GetAllApplicationTypes", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<ApplicationTypeDTO>());
                    }
                }
            });

            return list;
        }

    }
}
