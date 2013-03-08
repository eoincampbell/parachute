using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Parachute.Entities
{

    /// <summary>
    /// ParachuteSchemaChangeLog encapsulates the versioning information
    /// </summary>
    [Serializable]
    [XmlRoot("ParachuteSchemaChangeLog", Namespace = "", IsNullable = false)]
    public class ParachuteSchemaChangeLog
    {
        [Key]
        public int ParachuteSchemaChangeLogId { get; set; }
        public string MajorReleaseNumber { get; set; }
        public string MinorReleaseNumber { get; set; }
        public string PointReleaseNumber { get; set; }
       
        public string ScriptName { get; set; }

       
    }
}
