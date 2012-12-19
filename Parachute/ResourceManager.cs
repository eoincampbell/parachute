using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Parachute
{
    public class ResourceManager
    {
        public static string GetUsageMessage()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Parachute.Resources.Usage.txt"))
            {
                if (stream == null)
                    throw new ApplicationException("Unable to locate usage message resource. Aborting!");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetFullUsageMessage()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Parachute.Resources.FullUsage.txt"))
            {
                if (stream == null)
                    throw new ApplicationException("Unable to locate full usage message resource. Aborting!");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static string GetVersionInformationMessage()
        {
            string versionInfo;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Parachute.Resources.VersionInfo.txt"))
            {
                if (stream == null)
                    throw new ApplicationException("Unable to locate version information message resource. Aborting!");

                using (var reader = new StreamReader(stream))
                {
                    versionInfo = reader.ReadToEnd();
                }
            }

            var ass = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(ass.Location);
            var prodver = fvi.ProductVersion;
            var filever = fvi.FileVersion;
            var version = ass.GetName().Version;

            var output = versionInfo.Replace("$VERSION$", version.ToString())
                .Replace("$FILEVERSION$", filever)
                .Replace("$PRODUCTVERSION$", prodver);

            return output;
        }
    }
}
