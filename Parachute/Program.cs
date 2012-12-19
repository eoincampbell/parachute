using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Parachute
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string a = "--server (local) --database master --username sa --password epiosql --verbose";
            //string a = "-s (local) -d master -u sa -p epiosql -v";
            string a = "-c Server=(local);Database=master;Trusted_Connection=True;MultipleActiveResultSets=true; -v";

            ArgumentParser parser = new ArgumentParser(a.Split(' '));
            
            ParachuteSettings settings = parser.ParseSettings();
            
            settings.Validate();

            Console.ReadLine();
        }
    }
}
