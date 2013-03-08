using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Parachute.Entities
{


    /// <summary>
    /// ParachuteAppliedScriptsLog contains the single script information applied to the Database
    /// </summary>
    [Serializable]
    [XmlRoot("ParachuteAppliedScriptsLog", Namespace = "", IsNullable = false)]
    public class ParachuteAppliedScriptsLog
    {
         [Key]
        public int ParachuteAppliedScriptsLogId { get; set; }

        public string ScriptName { get; set; }
        public string Hash { get; set; }
        public string SchemaVersion { get; set; }

        
        public DateTime DateApplied { get; set; }
        public string AppliedBy { get; set; }

       
    }
}
