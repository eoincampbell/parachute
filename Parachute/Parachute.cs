using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parachute.DataAccess;
using Parachute.Entities;
using Parachute.Logic;

namespace Parachute
{
    public class Parachute
    {
        private const string a = "-s (local) -d Sandbox -u sa -p epiosql --loglevel 4 --console --setup -f ConfigurationFiles\\configtest.xml";

        public ParachuteSettings Settings { get; private set; }

        public void Start(string [] args)
        {
            //Parses command line settings to decide how to run application.
            Settings = ParachuteSettings.GetSettings(a.Split(' '));
            if (Settings.ExitNow || !Settings.IsValid())
            {
                TraceHelper.Warning(Settings.ExitMessage);
                throw new ParachuteException("Aborting. Settings error.");
            }

            //Validate Configuration File.
            var loader = new ScriptInformationLoader(Settings.ConfigFilePath);
            var scriptInfo = loader.Load();


            //Connect To Database
            using (var sqlManager = new SqlConnectionManager(Settings.ConnectionString))
            {
                sqlManager.InfoOrErrorMessage += SqlManagerOnInfoOrErrorMessage;

                ConfigureDatabase(sqlManager);
                
                //Logic Flow from here down...

                //Query the Schema Change Log Tables.
                SchemaVersion currentVersion = sqlManager.GetCurrentSchemaVersion();

                //Query the ScriptInfo Collection & Pull the Script Location for the Schema Directory...
                var schemaScriptLocation = scriptInfo.ScriptLocations.SingleOrDefault(fd => fd.ContainsSchemaScripts);
                //If there is one.
                //Pass that Off for processing.
                if(schemaScriptLocation != null)
                {
                    currentVersion = ApplySchemaChangesToDatabase(sqlManager, currentVersion, schemaScriptLocation);
                }
            }






        exitpoint:
            Trace.Flush();
        }

        private SchemaVersion ApplySchemaChangesToDatabase(SqlConnectionManager sqlManager, SchemaVersion currentVersion, ScriptLocation schemaScriptLocation)
        {
            //Ensure the scripts are ordered alphanumerically ascending
            foreach(var script in schemaScriptLocation.ScriptFiles.OrderBy(s => s))
            {
                var fileSchemaVersion = GetSchemaVersionFromSchemaScriptFileName(script);

                if (fileSchemaVersion > currentVersion)
                {
                    //If the file's schema version is greater than the currentVersion,
                    TraceHelper.Info("Applying '{0}'", script);
                    sqlManager.ExecuteSchemaFile(script, fileSchemaVersion);
                    
                    //Set that to be our new Current Version.
                    currentVersion = fileSchemaVersion;
                }
                else
                {
                    TraceHelper.Verbose("Skipping '{0}' - Current Version is '{1}'", script, currentVersion);
                }
            }

            //Return the new "Current" Schema Version
            return currentVersion;

        }

        private SchemaVersion GetSchemaVersionFromSchemaScriptFileName(string fileName)
        {
            var file = Path.GetFileName(fileName);

            return SchemaVersion.Parse(file);
        }

        private void ConfigureDatabase(SqlConnectionManager sqlManager)
        {
            //Check if the database is configured for Parachute (i.e. have the change logs been added).
            if (sqlManager.IsDatabaseConfiguredForParachute()) return;
            
            //If not... are we in setup mode
            if (Settings.SetupDatabase)
            {
                if (!sqlManager.SetupDatabase())
                {
                    throw new ParachuteException("Aborting. Failed to setup database");
                }
            }
            else
            {
                TraceHelper.Error("Database is not configured for Parachute");
                TraceHelper.Error("Re-run application with --setup switch to install Parachute ChangeLog Tables");
                throw new ParachuteException("Aborting. Database does not support Parachute");
            }
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
