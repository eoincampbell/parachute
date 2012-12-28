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
using Parachute.Exceptions;
using Parachute.Utilities;

namespace Parachute
{
    public class Parachute : IDisposable
    {
        private const string a = "-s (local) -d Sandbox -u sa -p epiosql --loglevel 4 --console --setup -f ConfigurationFiles\\configtest.xml";

        public ParachuteSettings Settings { get; set; }
        public SqlConnectionManager SqlManager { get; set; }
        public ScriptInformationLoader ScriptInfoLoader { get; set; }

        public Parachute(string [] args)
        {
            Settings = ParachuteSettings.GetSettings(a.Split(' '));
            Settings.Validate();

            ScriptInfoLoader = new ScriptInformationLoader(Settings.ConfigFilePath);

            SqlManager = new SqlConnectionManager(Settings.ConnectionString);
        }

        public void Run()
        {
            var scriptInfo = ScriptInfoLoader.Load();


            SqlManager.InfoOrErrorMessage += SqlManagerOnInfoOrErrorMessage;

            //Setup the database if needs be.
            var currentVersion = ConfigureDatabase(SqlManager);

            currentVersion = ApplySchemaChangesToDatabase(SqlManager, currentVersion, scriptInfo);

            ApplyScriptsToDatabase(SqlManager, currentVersion, scriptInfo);
            
            Trace.Flush();
        }

        private static void ApplyScriptsToDatabase(SqlConnectionManager sqlManager, SchemaVersion currentVersion, ScriptInformation scriptInfo)
        {
            foreach (var location in scriptInfo.ScriptLocations.Where(sl => !sl.ContainsSchemaScripts))
            {
                //foreach file in the location
                    // is directory marked as "runOnce"
                        //if yes
                            //check if file applied before in a previous version.
                                //if no
                                    // Apply that file & log it
                                //if yes
                                    //skip it
                        //if not
                            //run that file regardless

                
                //foreach directory in the location
                    // Recursively repeat.
            }
        }

        private static SchemaVersion ApplySchemaChangesToDatabase(SqlConnectionManager sqlManager, SchemaVersion currentVersion, ScriptInformation scriptInfo)
        {
            //Query the ScriptInfo Collection & Pull the Script Location for the Schema Directory...
            var schemaScriptLocation = scriptInfo.ScriptLocations.FirstOrDefault(fd => fd.ContainsSchemaScripts);
            //If there is one.
            //Pass that Off for processing.
            if (schemaScriptLocation != null)
            {
                //Ensure the scripts are ordered alphanumerically ascending
                foreach (var script in schemaScriptLocation.ScriptFiles.OrderBy(s => s))
                {
                    var fileSchemaVersion = script.ToSchemaVersion();

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
            }

            //Return the new "Current" Schema Version
            return currentVersion;

        }

       

        private SchemaVersion ConfigureDatabase(SqlConnectionManager sqlManager)
        {
            if (!sqlManager.IsDatabaseConfiguredForParachute() && Settings.SetupDatabase)
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

            return sqlManager.GetCurrentSchemaVersion();
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

        public void Dispose()
        {
            SqlManager.Dispose();
        }
    }
}
