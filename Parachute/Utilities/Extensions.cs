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
        public static SchemaVersion ToSchemaVersion(this string versionString)
        {
            var file = Path.GetFileName(versionString);

            return SchemaVersion.Parse(file);
        }
    }
}
