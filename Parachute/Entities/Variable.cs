using System;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    [Serializable]
    [XmlRoot("variable", Namespace = "", IsNullable = false)]
    public class Variable
    {
        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }
    }
}