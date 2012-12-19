using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parachute
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
            if (!Arguments.Any())
            {
                Console.WriteLine(ResourceManager.GetUsageMessage());
                return null;
            }

            ConfigureOutput();

            ParachuteSettings settings = new ParachuteSettings();

            for (var i = 0; i < Arguments.Count(); i++)
            {
                switch (Arguments[i])
                {
                    case "--help":
                        Console.WriteLine(ResourceManager.GetFullUsageMessage());
                        return null;
                    case "--version":
                        Console.WriteLine(ResourceManager.GetVersionInformationMessage());
                        return null;
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
                    case "--source":
                    case "-r":
                        settings.SqlScriptsPath = Arguments[++i];
                        break;
                }
            }

            return settings;
        }

        private void ConfigureOutput()
        {
            if (Arguments.Any(arg => arg.Equals("--verbose") || arg.Equals("-v")))
            {
                Trace.Listeners.Add(new ConsoleTraceListener(true));
            }

        }
    }
}
