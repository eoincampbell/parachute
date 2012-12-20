using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    [Serializable]
    [XmlRoot("script", Namespace = "", IsNullable = false)]
    public class Script
    {
        public Script()
        {
            Variables = new Collection<Variable>();
        }
        
        [XmlAttribute("scriptName")]
        public string ScriptName { get; set; }
        
        [XmlArray("variables")]
        [XmlArrayItem("variable")]
        public Collection<Variable> Variables { get; set; }
    }
}