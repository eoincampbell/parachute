using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Parachute.Exceptions;

namespace Parachute.Managers
{
    /// <summary>
    /// Utility class for parsing command line arguments and generating a settings object.
    /// </summary>
    public class CommandLineArgumentManager
    {
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        private List<string> Arguments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgumentManager" /> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public CommandLineArgumentManager(IEnumerable<string> args)
        {
            Arguments = args.ToList();
        }

        /// <summary>
        /// Parses the settings.
        /// </summary>
        /// <returns>A populated <see cref="ParachuteSettings"/> objects</returns>
        public ParachuteSettings ParseSettings()
        {
            var settings = new ParachuteSettings();

            if (!Arguments.Any())
            {
                throw new ParachuteAbortException(ResourceManager.GetUsageMessage());
            }

            ConfigureOutput();

            ConfigureSettings(ref settings);

            return settings;
        }


        /// <summary>
        /// Gets the trace level.
        /// </summary>
        /// <param name="setting">The input setting.</param>
        /// <returns></returns>
        private static TraceLevel GetLevel(string setting)
        {
            int level;
            if (int.TryParse(setting, out level) && level >= 1 && level <= 4)
            {
                return (TraceLevel)level;
            }
            return TraceLevel.Info;
        }

        /// <summary>
        /// Configures the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        private void ConfigureSettings(ref ParachuteSettings settings)
        {
            for (var i = 0; i < Arguments.Count(); i++)
            {
                switch (Arguments[i])
                {
                    case "--help":
                        throw new ParachuteAbortException(ResourceManager.GetFullUsageMessage());
                    case "--version":
                        throw new ParachuteAbortException(ResourceManager.GetVersionInformationMessage());
                    case "-s":
                    case "--server":
                        settings.Server = Arguments[++i];
                        break;
                    case "-d":
                    case "--database":
                        settings.Database = Arguments[++i];
                        break;
                    case "-u":
                    case "--username":
                        settings.Username = Arguments[++i];
                        break;
                    case "-p":
                    case "--password":
                        settings.Password = Arguments[++i];
                        break;
                    case "-c":
                    case "--connection":
                        settings.ConnectionString = Arguments[++i];
                        break;
                    case "--setup":
                        settings.SetupDatabase = true;
                        break;
                    case "--configfile":
                    case "-f":
                        settings.ConfigFilePath = Arguments[++i];
                        break;
                    case "-t":
                    case "--test":
                        settings.TestMode = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Configures the output mechanism.
        /// </summary>
        private void ConfigureOutput()
        {
            for (var i = 0; i < Arguments.Count(); i++)
            {
                switch (Arguments[i])
                {
                    case "--console":
                        Trace.Listeners.Add(new ConsoleTraceListener(true));
                        break;
                    case "--logfile":
                    case "-l":
                        Trace.Listeners.Add(new TextWriterTraceListener(Arguments[++i]));
                        break;
                    case "--loglevel":
                        TraceHelper.LoggingSwitch.Level = GetLevel(Arguments[++i]);
                        break;
                }
            }
        }
    }
}
