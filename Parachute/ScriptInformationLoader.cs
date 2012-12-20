using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Parachute.Entities;

namespace Parachute
{
    public class ScriptInformationLoader : IDisposable
    {
        public string FileName { get; private set; }
        public ScriptInformationLoader  (string filename)
        {
            FileName = filename;
        }

        public ScriptInformation Load()
        {
            
            ScriptInformation info;
            try
            {
                TraceHelper.Info(string.Format("Attempting to load script info from {0}", FileName));
                using (var strm = File.OpenRead(FileName))
                {
                    using (var reader = new StreamReader(strm))
                    {
                        var serializer = new XmlSerializer(typeof (ScriptInformation));

                        info = serializer.Deserialize(reader) as ScriptInformation;
                    }
                }
            }
            catch(Exception ex)
            {
                TraceHelper.Error(ex.Message);
                TraceHelper.Error(ex.ToString());
                return null;
            }


            /************************************************************
             * Validation
             */

            TraceHelper.Info("Validating Script Configuration File '{0}'", FileName);

            if (info == null)
            {
                TraceHelper.Error("Configuration file '{0}' could not be parsed. See example files in GitHub Wiki.", FileName);
                return null;
            }

            if(info.ScriptLocations.Count < 0)
            {
                TraceHelper.Error("Configuration file '{0}' contains no <scriptLocation> elements. Exiting", FileName);
                return null;
            }

            if(info.ScriptLocations.Count(l => l.ContainsSchemaScripts) > 1)
            {
                TraceHelper.Error("Configuration file '{0}' contains more than one <scriptLocation> with containsSchemaScripts=\"true\". Exiting", FileName);
                return null;
            }

            foreach(var location in info.ScriptLocations)
            {
                TraceHelper.Info("Validating Location '{0}' ", location.Path);
                TraceHelper.Verbose("> RunOnce: {0}", location.RunOnce);
                TraceHelper.Verbose("> Recursive: {0}", location.Recursive);
                TraceHelper.Verbose("> ContainsSchemaScripts: {0}", location.ContainsSchemaScripts);
                TraceHelper.Verbose("> Scripts With Variables #: {0}", location.Scripts.Count);

                if(!Directory.Exists(location.Path))
                {
                    TraceHelper.Error("Configuration file contains an invalid path '{0}'", location.Path);
                    TraceHelper.Error("Script location paths must be a valid directory. Exiting!");
                    return null;
                }
            }

            return info;
        }

        public void Dispose()
        {
            

        }
    }
}
