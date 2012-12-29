using System.Diagnostics;
using System.Reflection;

namespace Parachute.Managers
{
    /// <summary>
    /// Helper class to retrieve resource strings from the ResourceManager.
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// Gets the usage message.
        /// </summary>
        /// <returns>The usage message.</returns>
        public static string GetUsageMessage()
        {
            return Resources.Parachute.Usage;
        }

        /// <summary>
        /// Gets the full usage message.
        /// </summary>
        /// <returns>The full usage message.</returns>
        public static string GetFullUsageMessage()
        {
            return Resources.Parachute.FullUsage;
        }

        /// <summary>
        /// Gets the version information message.
        /// </summary>
        /// <returns>The version information message.</returns>
        public static string GetVersionInformationMessage()
        {
            var versionInfo = Resources.Parachute.VersionInfo;

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

        /// <summary>
        /// Gets the change log creation script.
        /// </summary>
        /// <returns>the change log table creation scripts</returns>
        public static string GetChangeLogCreationScript()
        {
            return Resources.Parachute.ChangeLogCreationScript;
        }

        /// <summary>
        /// Gets the change log exists script.
        /// </summary>
        /// <returns>the change log exists script.</returns>
        public static string GetChangeLogExistsScript()
        {
            return Resources.Parachute.ChangeLogExistsScript;
        }
    }
}
