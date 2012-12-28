﻿using System.Diagnostics;
using System.Reflection;

namespace Parachute.Utilities
{
    public class ResourceHelper
    {
        public static string GetUsageMessage()
        {
            return Resources.Parachute.Usage;
        }

        public static string GetFullUsageMessage()
        {
            return Resources.Parachute.FullUsage;
        }

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

        public static string GetChangeLogCreationScript()
        {
            return Resources.Parachute.ChangeLogCreationScript;
        }

        public static string GetChangeLogExistsScript()
        {
            return Resources.Parachute.ChangeLogExistsScript;
        }
    }
}