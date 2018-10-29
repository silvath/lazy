using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace lazy.Service
{
    public class ConsoleService
    {
        public static string Execute(string command, string arguments = null, string workingDirectory = null)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = command;
            if (!string.IsNullOrEmpty(arguments))
                startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            if (!string.IsNullOrEmpty(workingDirectory))
                startInfo.WorkingDirectory = workingDirectory;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string data = process.StandardOutput.ReadToEnd();
            return(data);
        }
    }
}
