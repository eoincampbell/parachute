using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parachute
{
    public class ParachuteSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public string SqlScriptsPath { get; set; }
        public bool SetupDatabase { get; set; }

        public bool Validate()
        {
            if(!string.IsNullOrEmpty(ConnectionString))
            {
                Trace.WriteLine("===================================================================================");
                Trace.WriteLine("Connecting Using ConnectionString...");
                Trace.WriteLine(string.Format("ConnectionString: {0}", ConnectionString));
                
                if(!SqlManager.TestConnection(ConnectionString))
                {
                    Console.WriteLine("Failed to Connect To Database. Exiting!");
                    return false;
                }
            }
            else
            {
                //Use CmdLine Params
                Trace.WriteLine("===================================================================================");
                Trace.WriteLine("Connecting Using CmdLine Arguments...");
                Trace.WriteLine(string.Format("Server:    {0}", Server));
                Trace.WriteLine(string.Format("Database:  {0}", Database));
                Trace.WriteLine(string.Format("Username:  {0}", Username));
                Trace.WriteLine(string.Format("Password:  {0}", Password));
                
                string connstr = SqlManager.BuildConnectionString(Server, Database, Username, Password);

                if (!SqlManager.TestConnection(connstr))
                {
                    Console.WriteLine("Failed to Connect To Database. Exiting!");
                    return false;
                }
            }

            return true;
        }
    }
}
