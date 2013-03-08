using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Parachute.Managers;

namespace Parachute.Utils
{
    public static class PowerShellIntegration
    {


        public static void RunScript(string scriptText, params string[] args)
        {
            // create Powershell runspace
            var runspace = RunspaceFactory.CreateRunspace();

            // open it
            runspace.Open();

            // create a pipeline and feed it the script text
            var pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // execute the script
            var results = pipeline.Invoke(args.ToList());

            // close the runspace
            runspace.Close();

            // convert the script result into a single string
            var stringBuilder = new StringBuilder();
            
            foreach (PSObject obj in results)
                stringBuilder.AppendLine(obj.ToString());

            TraceHelper.Verbose(stringBuilder.ToString());
        }
    }
}
