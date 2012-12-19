using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Parachute
{
    public class SqlManager
    {
        public static string BuildConnectionString(string server, string database, string username, string password)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();

            scsb.ApplicationName = "Parachute";
            scsb.DataSource = server;
            scsb.InitialCatalog = database;
            scsb.UserID = username;
            scsb.Password = password;

            return scsb.ConnectionString;
        }

        public static bool TestConnection(string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT 1", conn))
                    {
                        return ((int) cmd.ExecuteScalar() == 1);
                    }
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine("===================================================================================");
                Console.WriteLine(ex.Message);
                Trace.WriteLine(ex.ToString());
            }

            return false;
        }
    }
}
