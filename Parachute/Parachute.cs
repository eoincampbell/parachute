using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parachute.Logic;

namespace Parachute
{
    public class Parachute
    {
        public void Start(string [] args)
        {
            //string a = "--server (local) --database Sandbox --username sa --password epiosql --loglevel 4 --console --setup";
            //const string a = "-s (local) -d Sandbox -u sa -p epiosql --loglevel 3 --console --setup -f ConfigExample.xml";
            const string a = "-s (local) -d Sandbox -u sa -p epiosql --loglevel 4 --console --setup -f ConfigurationFiles\\configtest.xml";
            //string a = "-c Server=(local);Database=master;Trusted_Connection=True;MultipleActiveResultSets=true; -v";
            //string a = "--version";

            //Parses command line settings to decide how to run application.
            var settings = ParachuteSettings.GetSettings(a.Split(' '));
            if (settings.ExitNow || !settings.IsValid())
            {
                TraceHelper.Warning(settings.ExitMessage);
                throw new ParachuteException("Aborting. Settings error.");
            }

            //Start Doing Work...
            var loader = new ScriptInformationLoader(settings.ConfigFilePath);
            var scriptInfo = loader.Load();

            using (var sqlManager = new SqlConnectionManager(settings.ConnectionString))
            {
                sqlManager.InfoOrErrorMessage += SqlManagerOnInfoOrErrorMessage;

                //Check if the database is configured for Parachute (i.e. have the change logs been added).
                var databaseIsConfigured = sqlManager.IsDatabaseConfiguredForParachute();

                if (!databaseIsConfigured)
                {
                    //If not... are we in setup mode
                    if (settings.SetupDatabase)
                    {
                        //If yes, create the db change tracking table
                        if (!sqlManager.SetupDatabase())
                        {
                            throw new ParachuteException("Aborting. Failed to setup database");
                        }
                    }
                    else
                    {
                        //If not, exit 
                        TraceHelper.Error("Database is not configured for Parachute");
                        TraceHelper.Error("Re-run application with --setup switch to install Parachute ChangeLog Tables");
                        throw new ParachuteException("Aborting. Database does not support Parachute");
                    }
                }

                //Next... Apply the Schema Changes... Schema Files are run-once only

                //Load up the list of files/folders to be executed ?
                //Start iterating through them
                //Execute them in order.

                int x = 1 + 2;

            }






        exitpoint:
            Trace.Flush();
        }

        private static void SqlManagerOnInfoOrErrorMessage(object sender, SqlError sqlError)
        {
            LogInfoOrErrorMessage(sqlError);
        }

        private static void LogInfoOrErrorMessage(SqlError error)
        {
            if (error == null) return;

            if (error.Class > 10)
            {
                TraceHelper.Error("Msg {0}, Level {1}, State {2}, Line {3}{4}{5}", error.Number, error.Class, error.State, error.LineNumber, Environment.NewLine, error.Message);
            }
            else
            {
                TraceHelper.Info("{0}", error.Message);
            }
        }
    }
}
