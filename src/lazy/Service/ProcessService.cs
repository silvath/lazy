using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace lazy.Service
{
    public class ProcessService
    {
        private static string _codePath;
        public static bool IsConfigurated()
        {
            if (_codePath == null)
            {
                string response = ProcessService.Execute("where", "code");
                if (response.Contains("INFO:"))
                {
                    _codePath = "";
                }
                else
                {
                    _codePath = "";
                    string[] lines = response.Split("\r\n");
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line))
                            continue;
                        _codePath = line;
                        break;
                    }
                }
            }
            return (!string.IsNullOrEmpty(_codePath));
        }
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

        public static void OpenCode(string workingDirectory)
        {
            if (!IsConfigurated())
                return;
            string command = _codePath;
            string arguments = ".";
            ProcessStartInfo startInfo = new ProcessStartInfo(command, arguments);
            startInfo.FileName = command;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.UseShellExecute = false;
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
