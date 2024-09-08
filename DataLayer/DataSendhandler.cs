using DataAccessLayer;
using Microsoft.Data.SqlClient;


namespace DataLayerCore.Datahandler
{
    internal class DataSendhandler
    {
        public delegate Task RequestHandler(SqlConnection Connection, SqlCommand Command);
        public delegate void ExceptionHandler(Exception ex);

        private static void DefaultExceptionHandle()
        {

        }

        public static async Task handle(string StoredProcedure, RequestHandler handler)
        {
            await handle(StoredProcedure, handler, null);
        }

        public static async Task  handle(string StoredProcedure, RequestHandler handler, ExceptionHandler? ExHandler)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand Command = new SqlCommand(StoredProcedure, connection))
                    {
                        Command.CommandType = System.Data.CommandType.StoredProcedure;
                        await handler.Invoke(connection, Command);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExHandler is not null)
                    ExHandler.Invoke(ex);
                else
                    DefaultExceptionHandle();
            }

        }
    }
}
