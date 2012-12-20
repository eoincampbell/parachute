using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    [Serializable]
    [XmlRoot("scriptLocation", Namespace = "", IsNullable = false)]
    public class ScriptLocation
    {
        public ScriptLocation()
        {
            Scripts = new Collection<Script>();
            RunOnce = true;
            Recursive = false;
            ContainsSchemaScripts = false;
        }

        [XmlAttribute("recursive")]
        public bool Recursive { get; set; }

        [XmlAttribute("containsSchemaScripts")]
        public bool ContainsSchemaScripts { get; set; }

        [XmlAttribute("runOnce")]
        public bool RunOnce { get; set; }

        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlElement("script")]
        public Collection<Script> Scripts { get; set; }
    }
}