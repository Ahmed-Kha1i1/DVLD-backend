using DataLayerCore;
using DataLayerCore.ApplicationType;
using DataLayerCore.Datahandler;
using Microsoft.Data.SqlClient;

namespace DataLayerCore.Application
{
    public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };
    public class clsApplicationData
    {
        public static async Task<ApplicationDTO?> GetApplication(int ApplicationID)
        {

            ApplicationDTO? applicationDTO = null;
            await DataSendhandler.handle("SP_FindApplicationById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    if (Reader.Read())
                    {
                        applicationDTO = Reader.MapTo<ApplicationDTO>();

                    }
                }
            });

            return applicationDTO;
        }

        public static async Task<int?> AddNewApplication(ApplicationForCreateDTO application)
        {

            int? ApplicationID = null;
            await DataSendhandler.handle("SP_AddNewApplication", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@ApplicantPersonID", application.ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", application.ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", application.ApplicationStatus);
                Command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserID);

                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    ApplicationID = ID;
                }
            });


            return ApplicationID;
        }

        public static async Task<bool> UpdateApplication(int ApplicationID, ApplicationForUpdateDTO application)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateApplication", async (Connection, Command) =>
            {
                Connection.Open();

                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@ApplicantPersonID", application.ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
                Command.Parameters.AddWithValue("@ApplicationTypeID", application.ApplicationTypeID);
                Command.Parameters.AddWithValue("@ApplicationStatus", application.ApplicationStatus);
                Command.Parameters.AddWithValue("@LastStatusDate", application.LastStatusDate);
                Command.Parameters.AddWithValue("@PaidFees", application.PaidFees);
                Command.Parameters.AddWithValue("@CreatedByUserID", application.CreatedByUserID);

                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<bool> DeleteApplication(int ApplicationID)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_DeleteApplicationById", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                RowAffected = await Command.ExecuteNonQueryAsync();

            });

            return RowAffected > 0;
        }

        public static async Task<List<ApplicationDTO>> GetApplications()
        {
            var list = new List<ApplicationDTO>();
            await DataSendhandler.handle("SP_GetAllApplications", async (Connection, Command) =>
            {
                Connection.Open();
                using (SqlDataReader Reader = await Command.ExecuteReaderAsync())
                {
                    while (Reader.Read())
                    {
                        list.Add(Reader.MapTo<ApplicationDTO>());
                    }
                }
            });

            return list;
        }

        public static async Task<bool> IsApplicationExist(int ApplicationID)
        {
            bool IsFound = false;
            await DataSendhandler.handle("SP_IsApplicationExists", async (Connection, Command) =>
            {
                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                object? Result = await Command.ExecuteScalarAsync();
                IsFound = Result is not null;
            });

            return IsFound;
        }

        public static async Task<bool> UpdateStatus(int ApplicationID, enApplicationStatus ApplicationStatus)
        {
            int RowAffected = 0;
            await DataSendhandler.handle("SP_UpdateStatus", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
                Connection.Open();
                RowAffected = await Command.ExecuteNonQueryAsync();
            });

            return RowAffected > 0;
        }

        public static async Task<bool> DoesPersonHasActiveApplication(int ApplicantPersonID, enApplicationType ApplicationTypeID)
        {
            return await GetActiveApplicationID(ApplicantPersonID, ApplicationTypeID) != null;
        }

        public static async Task<int?> GetActiveApplicationID(int ApplicantPersonID, enApplicationType ApplicationTypeID)
        {
            int? ApplicationID = null;
            await DataSendhandler.handle("SP_GetActiveApplicationID", async (Connection, Command) =>
            {

                Connection.Open();
                Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    ApplicationID = ID;
                }
            });

            return ApplicationID;
        }

        public static async Task<int?> GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            int? ApplicationID = null;
            await DataSendhandler.handle("SP_GetActiveApplicationIDForLicenseClass", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


                Connection.Open();
                object? Result = await Command.ExecuteScalarAsync();

                if (Result is not null && int.TryParse(Result.ToString(), out int ID))
                {
                    ApplicationID = ID;
                }
            });

            return ApplicationID;
        }

        public static async Task<bool> IsApplicationCancelled(int ApplicationID)
        {
            bool IsCancelled = false;
            await DataSendhandler.handle("SP_IsApplicationCancelled", async (Connection, Command) =>
            {
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Connection.Open();

                object? result = await Command.ExecuteScalarAsync();

                if (result is not null)
                {
                    IsCancelled = true;
                }
            });

            return IsCancelled;
        }

    }
}
