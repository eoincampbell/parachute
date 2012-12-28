using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    /// <summary>
    /// Represents a single directory path of scripts in the xml document which 
    /// should be applied to the database.
    /// </summary>
    [Serializable]
    [XmlRoot("scriptLocation", Namespace = "", IsNullable = false)]
    public class ScriptLocation
    {
        private const string Filter = "*.sql";

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptLocation" /> class.
        /// </summary>
        public ScriptLocation()
        {
            Scripts = new Collection<Script>();
            RunOnce = true;
            Recursive = false;
            ContainsSchemaScripts = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ScriptLocation" /> 
        /// directory should  recursively listed, and all scripts in all sub-directories 
        /// applied. Only applicable to for non-schema script locations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recursive; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("recursive")]
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this directory [contains schema scripts].
        /// </summary>
        /// <value>
        /// <c>true</c> if [contains schema scripts]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("containsSchemaScripts")]
        public bool ContainsSchemaScripts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the script should [run once] or 
        /// be re-run each time. Only applicable to for non-schema script locations.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [run once]; otherwise, <c>false</c>.
        /// </value>
        [XmlAttribute("runOnce")]
        public bool RunOnce { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [XmlAttribute("path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the scripts which have manual variable overrides.
        /// </summary>
        /// <value>
        /// The scripts.
        /// </value>
        [XmlElement("script")]
        public Collection<Script> Scripts { get; set; }


        /// <summary>
        /// Gets the script files in the currenct directory location.
        /// </summary>
        /// <value>
        /// The script files.
        /// </value>
        public IEnumerable<string> ScriptFiles
        {
            get
            {
                var so = (Recursive && !ContainsSchemaScripts)
                             ? SearchOption.AllDirectories
                             : SearchOption.TopDirectoryOnly;

                return Directory.GetFiles(Path,Filter, so);
            }
        }
    }
}