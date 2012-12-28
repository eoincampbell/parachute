using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    /// <summary>
    /// Represents an xml document Script Information which should be applied to the database.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "scriptInfo", Namespace = "", IsNullable = false)]
    public class ScriptInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptInformation" /> class.
        /// </summary>
        public ScriptInformation()
        {
            ScriptLocations = new Collection<ScriptLocation>();    
        }

        /// <summary>
        /// Gets or sets the script locations.
        /// </summary>
        /// <value>
        /// The script locations.
        /// </value>
        [XmlElement("scriptLocations")]
        public Collection<ScriptLocation> ScriptLocations { get; set; }
    }
}
