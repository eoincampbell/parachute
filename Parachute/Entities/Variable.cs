using System;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    /// <summary>
    /// A class representing a single variable which should be overridden 
    /// with a specific value at script application time.
    /// </summary>
    [Serializable]
    [XmlRoot("variable", Namespace = "", IsNullable = false)]
    public class Variable
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [XmlAttribute("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        [XmlAttribute("defaultValue")]
        public string DefaultValue { get; set; }
    }
}