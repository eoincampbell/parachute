using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parachute
{
    public static class TraceHelper
    {
        static TraceHelper()
        {
            
        }

        public static TraceSwitch LoggingSwitch = new TraceSwitch("ParachuteTrace", "ParachuteTrace", "3");


        public static void Error(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceError, string.Format("Error     " +format, args));
        }

        public static void Warning(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceWarning, string.Format("Warning   " + format, args));
        } 

        public static void Info(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceInfo, string.Format("Info      " + format, args));
        }

        public static void Verbose(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceVerbose, string.Format("Verbose   " + format, args));
        }
    }
}
