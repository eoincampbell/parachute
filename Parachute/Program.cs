using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Parachute
{
    public class Program
    {
        public static TraceSwitch LoggingSwitch = new TraceSwitch("ParachuteTrace", "ParachuteTrace", "3");

        static void Main(string[] args)
        {
            Trace.AutoFlush = true;
            LoggingSwitch.Level = TraceLevel.Info;

            //string a = "--server (local) --database Sandbox --username sa --password epiosql --loglevel 4 --console --setup";
            string a = "-s (local) -d Sandbox -u sa -p epiosql --loglevel 4 --console --setup --source config.xml";
            //string a = "-c Server=(local);Database=master;Trusted_Connection=True;MultipleActiveResultSets=true; -v";
            //string a = "--version";

            //Parses command line settings to decide how to run application.
            var parser = new ArgumentParser(a.Split(' '));
            var settings = parser.ParseSettings();
            if (settings.ExitNow)
            {
                Trace.WriteLineIf(LoggingSwitch.TraceWarning,settings.ExitMessage);
                goto exitpoint;
            }
            
            //Validates the different settings.
            if(!settings.IsValid())
            {
                Trace.WriteLine(LoggingSwitch.TraceWarning, settings.ExitMessage);
                goto exitpoint;
            }

            //Start Doing Work...

            //Check if the database is configured for Parachute (i.e. have the change logs been added).

            //Load up all the scripts we might need to run...
            



            //Connect to Database and see if the ChangeLog Tables are present.
            //If not... are we in setup mode
            //If yes, create the db change tracking table
            //If not, exit 
            using (var sqlManager = new SqlConnectionManager(settings.ConnectionString))
            {
                sqlManager.InfoOrErrorMessage += SqlManagerOnInfoOrErrorMessage;

                if(settings.SetupDatabase)
                {
                    if(!sqlManager.SetupDatabase())
                    {
                        goto exitpoint;
                    }
                }
            }
            
            
            //Next... Apply the Schema Changes... Schema Files are run-once only
            
            
            
            
            //Load up the list of files/folders to be executed ?
                //Start iterating through them
                    //Execute them in order.


            

exitpoint:
            Trace.Flush();
            Console.ReadLine();
        }

        private static void SqlManagerOnInfoOrErrorMessage(object sender, SqlError sqlError)
        {
            LogInfoOrErrorMessage(sqlError);
        }

        private static void LogInfoOrErrorMessage(SqlError error)
        {
            if (error == null) throw new ArgumentNullException("error");

            string message;
            Trace.WriteLineIf(LoggingSwitch.TraceError, "===================================================================================");
            if (error.Class > 10)
            {
                message =string.Format("Msg {0}, Level {1}, State {2}, Line {3}{4}{5}", error.Number, error.Class, error.State, error.LineNumber, Environment.NewLine, error.Message);
                Trace.WriteLineIf(LoggingSwitch.TraceError, message);
            }
            else
            {
                message = string.Format("{0}", error.Message);
                Trace.WriteLineIf(LoggingSwitch.TraceInfo, message);
            }
        }
    }
}
