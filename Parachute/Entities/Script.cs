using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    /// <summary>
    /// Script Entity representing information about a single script in the configuration file
    /// </summary>
    [Serializable]
    [XmlRoot("script", Namespace = "", IsNullable = false)]
    public class Script
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.
        /// </summary>
        public Script()
        {
            Variables = new Collection<Variable>();
        }

        /// <summary>
        /// Gets or sets the name of the script.
        /// </summary>
        /// <value>
        /// The name of the script.
        /// </value>
        [XmlAttribute("scriptName")]
        public string ScriptName { get; set; }

        /// <summary>
        /// Gets or sets the variables.
        /// </summary>
        /// <value>
        /// The variables.
        /// </value>
        [XmlArray("variables")]
        [XmlArrayItem("variable")]
        public Collection<Variable> Variables { get; set; }
    }
}