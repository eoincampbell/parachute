using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parachute.Entities;

namespace Parachute.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Converts a string to a schema version if the format is compatible
        /// </summary>
        /// <param name="versionString">The input version string.</param>
        /// <returns>The schema verison.</returns>
        public static SchemaVersion ToSchemaVersion(this string versionString)
        {
            var file = Path.GetFileName(versionString);

            return SchemaVersion.Parse(file);
        }
    }
}
