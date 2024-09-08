using System;
using System.Configuration;

namespace DataAccessLayer
{
    public class clsDataAccessSettings
    {
        //public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString;
        public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
    }
}
