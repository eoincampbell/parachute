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
using Parachute.Managers;

namespace Parachute
{
    public class Parachute : IDisposable
    {
        private const string a = "-s (localdb)\\v11.0 -d Sandbox -u sa -p epiosql --loglevel 4 --console --setup -f ConfigurationFiles\\configtest.xml --test";

        public ParachuteSettings Settings { get; set; }
        public DataManager SqlManager { get; set; }
        public ScriptConfigFileManager ScriptInfoLoader { get; set; }
        public FileIOManager IOManager { get; set; }

        public Parachute(string[] args)
        {
            Settings = ParachuteSettings.GetSettings(a.Split(' '));
            Settings.Validate();

            ScriptInfoLoader = new ScriptConfigFileManager(Settings.ConfigFilePath);

            IOManager = new FileIOManager(new SqlFileGoSplitter());

            //TODO: can be obtained with IoC...
            SqlManager = new DataManager(Settings.ConnectionString, Settings.TestMode
                ? (IParachuteCommand)new SingleTransactionCommand(Settings.ConnectionString, Settings.ExecutionMode)
                : new MultiTransactionCommand(Settings.ConnectionString));

            SqlManager.InfoOrErrorMessage += SqlManagerOnInfoOrErrorMessage;

        }

        public void Run()
        {
            var scriptInfo = ScriptInfoLoader.Load();
            var currentVersion = SqlManager.ConfigureDatabase(Settings.SetupDatabase);

            currentVersion = ApplySchemaChangesToDatabase(currentVersion, scriptInfo);
            ApplyScriptsToDatabase(currentVersion, scriptInfo);

            Trace.Flush();
        }

        private void ApplyScriptsToDatabase(SchemaVersion currentVersion, ScriptInformation scriptInfo)
        {
            foreach (var location in scriptInfo.ScriptLocations.Where(sl => !sl.ContainsSchemaScripts))
            {
                TraceHelper.Info("Applying Change Scripts In '{0}'", location.Path);
                TraceHelper.Info("\tRunOnce: '{0}'", location.RunOnce);
                TraceHelper.Info("\tRecursive: '{0}'", location.Recursive);

                ApplyScriptsToDatabaseRecursiveDirectoryWalk(currentVersion, location, location.AbsolutePath);
            }
        }

        private void ApplyScriptsToDatabaseRecursiveDirectoryWalk(SchemaVersion currentVersion, ScriptLocation location, string currentDirectoryPath)
        {
            foreach (var scriptFile in location.ScriptFiles.OrderBy(s => s))
            {
                var scriptNameToBeLogged = scriptFile.Replace(location.AbsolutePath, string.Empty);

                if (!location.RunOnce) //Can be run many times
                {
                    TraceHelper.Info("Applying Script '{0}'", scriptNameToBeLogged);

                    var scripts = IOManager.ReadSqlScripts(scriptFile);

                    var hash = IOManager.GetFileMD5Hash(scriptFile);

                    SqlManager.ExecuteScriptFile(scripts, scriptNameToBeLogged, hash, currentVersion);
                }
                else
                {
                    TraceHelper.Info("Applying Script '{0}'", scriptNameToBeLogged);

                    var scripts = IOManager.ReadSqlScripts(scriptFile);

                    var hash = IOManager.GetFileMD5Hash(scriptFile);

                    SqlManager.ExecuteScriptFile(scripts, scriptNameToBeLogged, hash, currentVersion);
                }
            }


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

            /*
            if (!location.Recursive) return;
            //foreach directory in the location
            foreach(var subDirectoryPath in Directory.GetDirectories(currentDirectoryPath))
            {
                // Recursively repeat.
                ApplyScriptsToDatabaseRecursiveDirectoryWalk(currentVersion, location, subDirectoryPath);
            }
             */
        }

        private SchemaVersion ApplySchemaChangesToDatabase(SchemaVersion currentVersion, ScriptInformation scriptInfo)
        {
            //Query the ScriptInfo Collection & Pull the Script Location for the Schema Directory...
            var schemaScriptLocation = scriptInfo.ScriptLocations.FirstOrDefault(fd => fd.ContainsSchemaScripts);
            //If there is one.
            //Pass that Off for processing.
            if (schemaScriptLocation != null)
            {
                //Ensure the scripts are ordered alphanumerically ascending
                foreach (var scriptFile in schemaScriptLocation.ScriptFiles.OrderBy(s => s))
                {
                    var fileSchemaVersion = scriptFile.ToSchemaVersion();

                    if (fileSchemaVersion > currentVersion)
                    {
                        var scriptNameToBeLogged = scriptFile.Replace(schemaScriptLocation.AbsolutePath, string.Empty);

                        //If the file's schema version is greater than the currentVersion,
                        TraceHelper.Info("Applying Schema Change '{0}'", scriptNameToBeLogged);

                        var scripts = IOManager.ReadSqlScripts(scriptFile);

                        SqlManager.ExecuteSchemaFile(scripts, scriptNameToBeLogged, fileSchemaVersion);

                        //Set that to be our new Current Version.
                        currentVersion = fileSchemaVersion;
                    }
                    else
                    {
                        TraceHelper.Verbose("Skipping '{0}' - Current Version is '{1}'", scriptFile, currentVersion);
                    }
                }
            }

            //Return the new "Current" Schema Version
            return currentVersion;

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
