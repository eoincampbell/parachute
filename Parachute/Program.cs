using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Parachute.Exceptions;
using Parachute.Managers;

namespace Parachute
{
    public class Program
    {   
        static int Main(string[] args)
        {
            int returnValue;

            using (var p = new Parachute(args))
            {
                try
                {
                    p.Run();
                    returnValue = 0;
                }
                catch(ParachuteAbortException paex)
                {
                    TraceHelper.Warning(paex.Message);
                    returnValue = 2;      
                }
                catch (ParachuteException pex)
                {
                    TraceHelper.Warning(pex.Message);
                    TraceHelper.Verbose(pex.ToString());
                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    TraceHelper.Verbose(ex.ToString());
                    returnValue = 1;
                }
            }

#if DEBUG
            //Pause at the end of execution in Dev Mode.
// ReSharper disable LocalizableElement
            Console.WriteLine("--- press any key to continue ---");
// ReSharper restore LocalizableElement
            Console.Read();
#endif


            return returnValue;
        }
    }
}
