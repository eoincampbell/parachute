using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parachute.Entities
{
    [Serializable]
    [XmlRoot(ElementName = "scriptInfo", Namespace = "", IsNullable = false)]
    public class ScriptInformation
    {
        public ScriptInformation()
        {
            ScriptLocations = new Collection<ScriptLocation>();    
        }

        [XmlElement("scriptLocations")]
        public Collection<ScriptLocation> ScriptLocations { get; set; }
    }
}
