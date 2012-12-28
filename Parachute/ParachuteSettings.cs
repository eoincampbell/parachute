using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parachute.DataAccess;
using Parachute.Utilities;

namespace Parachute
{
    public class ParachuteSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public string ConfigFilePath { get; set; }
        public bool SetupDatabase { get; set; }

        public void Validate()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                TraceHelper.Verbose("Connecting Using ConnectionString...");
                TraceHelper.Verbose("ConnectionString: {0}", ConnectionString);

                if (!SqlConnectionManager.TestConnection(ConnectionString))
                {
                    throw new ParachuteAbortException("Failed to Connect To Database.");
                }
            }
            else
            {
                //Use CmdLine Params
                TraceHelper.Verbose("Connecting Using CmdLine Arguments...");
                TraceHelper.Verbose("Server:    {0}", Server);
                TraceHelper.Verbose("Database:  {0}", Database);
                TraceHelper.Verbose("Username:  {0}", Username);
                TraceHelper.Verbose("Password:  {0}", Password);

                var connstr = SqlConnectionManager.BuildConnectionString(Server, Database, Username, Password);
                ConnectionString = connstr;
                if (!SqlConnectionManager.TestConnection(ConnectionString))
                {
                    throw new ParachuteAbortException("Failed to Connect To Database.");
                }
            }


            if(string.IsNullOrEmpty(ConfigFilePath))
            {
                throw new ParachuteAbortException(
                    "Config File Path is not specified.\r\nYou must specify a configfile using the -f switch.");
            }

            if (!File.Exists(ConfigFilePath))
            {
                throw new ParachuteAbortException(
                    "The specified config file does not exist.\r\nYou must specify a configfile using the -f switch.");
            }
        }


        public static ParachuteSettings GetSettings(string [] args)
        {
            var parser = new ArgumentParser(args);

            var settings = parser.ParseSettings();

            return settings;
        }
    }
}
