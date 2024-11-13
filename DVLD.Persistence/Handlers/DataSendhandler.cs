using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace DVLD.Persistence.Handlers
{
    public class DataSendhandler
    {
        public delegate Task RequestHandler(SqlConnection Connection, SqlCommand Command);
        public delegate void ExceptionHandler(Exception ex);
        private readonly ConnectionStrings _connectionStrings;
        public DataSendhandler(IOptionsSnapshot<ConnectionStrings> connectionStrings)
        {
            _connectionStrings = connectionStrings.Value;
        }

        private void DefaultExceptionHandle()
        {

        }

        public async Task Handle(string StoredProcedure, RequestHandler handler)
        {
            await Handle(StoredProcedure, handler, null);
        }

        public async Task Handle(string StoredProcedure, RequestHandler handler, ExceptionHandler? ExHandler)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionStrings.DefaultConnection))
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
