﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public string ConfigFilePath { get; set; }
        public bool SetupDatabase { get; set; }

        public bool ExitNow { get; set; }
        public string ExitMessage { get; set; }


        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                TraceHelper.Verbose("Connecting Using ConnectionString...");
                TraceHelper.Verbose("ConnectionString: {0}", ConnectionString);

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
                TraceHelper.Verbose("Connecting Using CmdLine Arguments...");
                TraceHelper.Verbose("Server:    {0}", Server);
                TraceHelper.Verbose("Database:  {0}", Database);
                TraceHelper.Verbose("Username:  {0}", Username);
                TraceHelper.Verbose("Password:  {0}", Password);

                var connstr = SqlConnectionManager.BuildConnectionString(Server, Database, Username, Password);
                ConnectionString = connstr;
                if (!SqlConnectionManager.TestConnection(ConnectionString))
                {
                    ExitMessage = "Failed to Connect To Database. Exiting!";
                    ExitNow = true;
                    return false;
                }
            }


            if(string.IsNullOrEmpty(ConfigFilePath))
            {
                ExitMessage = "Config File Path is not specified.\r\nYou must specify a configfile using the -f switch.\r\nExiting!";
                ExitNow = true;
                return false;
            }

            if (!File.Exists(ConfigFilePath))
            {
                ExitMessage = "The specified config file does not exist.\r\nYou must specify a configfile using the -f switch.\r\nExiting!";
                ExitNow = true;
                return false;
            }

            return true;
        }
    }
}
