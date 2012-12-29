using System.Diagnostics;

namespace Parachute.Managers
{
    /// <summary>
    /// Helper class for writing to the TraceListeners
    /// </summary>
    public static class TraceHelper
    {
        /// <summary>
        /// Initializes the <see cref="TraceHelper" /> class.
        /// </summary>
        static TraceHelper()
        {
            
        }

        /// <summary>
        /// The logging switch
        /// </summary>
        public static TraceSwitch LoggingSwitch = new TraceSwitch("ParachuteTrace", "ParachuteTrace", "3");


        /// <summary>
        /// Writes the specified message to the trace listener as an Error
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Error(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceError, string.Format("Error     " +format, args));
        }

        /// <summary>
        /// Writes the specified message to the trace listener as a Warning
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Warning(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceWarning, string.Format("Warning   " + format, args));
        }

        /// <summary>
        /// Writes the specified message to the trace listener as Information
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Info(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceInfo, string.Format("Info      " + format, args));
        }

        /// <summary>
        /// Writes the specified message to the trace listener as a Verbose message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public static void Verbose(string format, params object[] args)
        {
            Trace.WriteLineIf(LoggingSwitch.TraceVerbose, string.Format("Verbose   " + format, args));
        }
    }
}
