using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Parachute.Logic
{
    public class ArgumentParser
    {
        private List<string> Arguments { get; set; }

        public ArgumentParser(IEnumerable<string> args)
        {
            Arguments = args.ToList();
        }

        public ParachuteSettings ParseSettings()
        {
            var settings = new ParachuteSettings();

            if (!Arguments.Any())
            {
                settings.ExitMessage = ResourceHelper.GetUsageMessage();
                settings.ExitNow = true;
                return settings;
            }

            ConfigureOutput();

            ConfigureSettings(ref settings);

            return settings;
        }

        private TraceLevel GetLevel(string s)
        {
            int level;
            if(int.TryParse(s, out level) && level >= 1 && level <= 4)
            {
                return (TraceLevel) level;
            }
            return TraceLevel.Info;
        }

        private void ConfigureSettings(ref ParachuteSettings settings)
        {
            for (var i = 0; i < Arguments.Count(); i++)
            {
                switch (Arguments[i])
                {
                    case "--help":
                        settings.ExitMessage = ResourceHelper.GetFullUsageMessage();
                        settings.ExitNow = true;
                        return;
                    case "--version":
                        settings.ExitMessage = ResourceHelper.GetVersionInformationMessage();
                        settings.ExitNow = true;
                        return;
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
                }
            }
        }

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
