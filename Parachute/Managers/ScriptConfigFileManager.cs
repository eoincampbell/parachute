using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Parachute.Entities;
using Parachute.Exceptions;

namespace Parachute.Managers
{
    /// <summary>
    /// Utility Class for parsing & loading the script information xml file.
    /// </summary>
    public class ScriptConfigFileManager
    {
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; private set; }

        public ScriptConfigFileManager  (string filename)
        {
            FileName = filename;
        }

        /// <summary>
        /// Loads the data from file.
        /// </summary>
        /// <returns>The <see cref="ScriptInformation"/> object.</returns>
        /// <exception cref="ParachuteException">throws if xml cannot be parsed or loaded.</exception>
        private ScriptInformation LoadFromFile()
        {
            ScriptInformation info;

            try
            {
                TraceHelper.Info(string.Format("Attempting to load script info from {0}", FileName));
                using (var strm = File.OpenRead(FileName))
                {
                    using (var reader = new StreamReader(strm))
                    {
                        var serializer = new XmlSerializer(typeof(ScriptInformation));

                        info = serializer.Deserialize(reader) as ScriptInformation;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ParachuteException(ex.Message, ex);
            }
            return info;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <returns>The <see cref="ScriptInformation"/> object.</returns>
        public ScriptInformation Load()
        {
            ScriptInformation info = LoadFromFile();

            TraceHelper.Info("Validating Script Configuration File '{0}'", FileName);

            InformationChecks(info);

            foreach(var location in info.ScriptLocations)
            {
                LocationCheck(location);

                LocationCheckPath(location);

                LocationCheckPathContents(location);

                LocationCheckCustomScriptVariables(location);
            }

            return info;
        }

        /// <summary>
        /// Checks that the the <see cref="ScriptInformation"/> is valid.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <exception cref="ParachuteException">Aborting. Incorrect Configuration.</exception>
        private void InformationChecks(ScriptInformation info)
        {
            if (info == null)
            {
                TraceHelper.Error("Configuration file '{0}' could not be parsed. See example files in GitHub Wiki.", FileName);
                throw new ParachuteException("Aborting. Incorrect Configuration.");
            }

            if (info.ScriptLocations.Count < 0)
            {
                TraceHelper.Error("Configuration file '{0}' contains no <scriptLocation> elements.", FileName);
                throw new ParachuteException("Aborting. Incorrect Configuration.");
            }

            if (info.ScriptLocations.Count(l => l.ContainsSchemaScripts) > 1)
            {
                TraceHelper.Error("Configuration file '{0}' contains more than one <scriptLocation> with containsSchemaScripts=\"true\".", FileName);
                throw new ParachuteException("Aborting. Incorrect Configuration.");
            }
        }

        /// <summary>
        /// Checks that the custom script variables in the <see cref="ScriptLocation"/> correspond to valid scripts.
        /// </summary>
        /// <param name="location">The location.</param>
        private void LocationCheckCustomScriptVariables(ScriptLocation location)
        {
            if(!location.Scripts.Any())
            {
                TraceHelper.Verbose("Location '{0}' contains no custom script variables", location.Path);
                return;
            }

            var files = location.ScriptFiles.ToList();

            foreach(var script in location.Scripts)
            {
                if(files.Contains(script.ScriptName))
                {
                    TraceHelper.Verbose("> Custom Variables found for script '{0}'", script.ScriptName);
                }
                else
                {
                    TraceHelper.Warning("> Custom Variables found for non-existant script '{0}'", script.ScriptName);
                }
            }
        }

        /// <summary>
        /// Checks that the path specified in the <see cref="ScriptLocation"/> contain valid scripts.
        /// </summary>
        /// <param name="location">The location.</param>
        private void LocationCheckPathContents(ScriptLocation location)
        {
            if (!location.ContainsSchemaScripts) return;

            TraceHelper.Verbose("> Schema Script Directory Found");
            if (Directory.GetDirectories(location.Path).Any())
            {
                TraceHelper.Warning("> Schema Script Directory contains subdirectories which will be ignored.");
            }

            foreach (var filename in Directory.GetFiles(location.Path, "*.sql", SearchOption.TopDirectoryOnly).Select(Path.GetFileName))
            {
                TraceHelper.Verbose("> Validating Schema Script '{0}'", filename);
                LocationCheckSchemaFile(filename);
            }
        }

        /// <summary>
        /// Checks that the schema files located in a SchemaScript <see cref="ScriptLocation"/> follw the correct naming convention.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ParachuteException">Aborting. Invalid Schema Script Name.</exception>
        private void LocationCheckSchemaFile(string filename)
        {
            var r = new Regex(@"\d{2}\.\d{2}\.\d{4}..*");

            if (r.IsMatch(filename)) return;
            
            TraceHelper.Error("Schema Script '{0}' filename is not in the correct format. Schema Scripts", filename);
            TraceHelper.Error("the convention '01.23.4567.any _.character description.sql.");
            throw new ParachuteException("Aborting. Invalid Schema Script Name.");
        }

        /// <summary>
        /// Checks that the path specified in the <see cref="ScriptLocation"/> is valid.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <exception cref="ParachuteException">Aborting. Invalid Script Location.</exception>
        private void LocationCheckPath(ScriptLocation location)
        {
            if (Directory.Exists(location.Path)) return;

            TraceHelper.Error("Configuration file contains an invalid path '{0}'", location.Path);
            TraceHelper.Error("Script location paths must be a valid directory.");
            throw new ParachuteException("Aborting. Invalid Script Location.");
        }

        /// <summary>
        /// Prints basic information about the <see cref="ScriptLocation"/>
        /// </summary>
        /// <param name="location">The location.</param>
        private void LocationCheck(ScriptLocation location)
        {
            TraceHelper.Info("Validating Location '{0}' ", location.Path);
            TraceHelper.Verbose("> RunOnce: {0}", location.RunOnce);
            TraceHelper.Verbose("> Recursive: {0}", location.Recursive);
            TraceHelper.Verbose("> ContainsSchemaScripts: {0}", location.ContainsSchemaScripts);
            TraceHelper.Verbose("> Scripts With Variables #: {0}", location.Scripts.Count);
        }

    }
}
