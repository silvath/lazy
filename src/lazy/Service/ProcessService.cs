using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace lazy.Service
{
    public class ProcessService
    {
        public static void Open(string workingDirectory)
        {
            string command = "cmd";
            string arguments = "";
            ProcessStartInfo startInfo = new ProcessStartInfo(command, arguments);
            startInfo.FileName = command;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }

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
