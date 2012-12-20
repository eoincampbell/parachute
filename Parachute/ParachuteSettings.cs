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

        public bool ExitNow { get; set; }
        public string ExitMessage { get; set; }


        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, "===================================================================================");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, "Connecting Using ConnectionString...");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, string.Format("ConnectionString: {0}", ConnectionString));

                if (!SqlConnectionManager.TestConnection(ConnectionString))
                {
                    ExitMessage = "Failed to Connect To Database. Exiting!";
                    ExitNow = true;
                    return false;
                }
            }
            else
            {
                //Use CmdLine Params
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, "===================================================================================");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, "Connecting Using CmdLine Arguments...");
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, string.Format("Server:    {0}", Server));
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, string.Format("Database:  {0}", Database));
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, string.Format("Username:  {0}", Username));
                Trace.WriteLineIf(Program.LoggingSwitch.TraceVerbose, string.Format("Password:  {0}", Password));

                var connstr = SqlConnectionManager.BuildConnectionString(Server, Database, Username, Password);
                ConnectionString = connstr;
                if (!SqlConnectionManager.TestConnection(ConnectionString))
                {
                    ExitMessage = "Failed to Connect To Database. Exiting!";
                    ExitNow = true;
                    return false;
                }
            }

            return true;
        }
    }
}
