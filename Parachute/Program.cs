using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Parachute.Logic;

namespace Parachute
{
    public class Program
    {
        
        static int Main(string[] args)
        {
            try
            {
                var p = new Parachute();
                p.Start(args);
                Console.Read();
                return 0;
            }
            catch(Exception ex)
            {
                TraceHelper.Verbose(ex.ToString());
                Console.Read();
                return 1;
            }

            

        }

       
    }
}
