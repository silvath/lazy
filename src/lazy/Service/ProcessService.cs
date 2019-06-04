using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace lazy.Service
{
    public class ProcessService
    {
        private static string _vsPath;//@"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\devenv.exe";
        private static string _codePath;
        public static bool IsConfiguratedVisualStudio()
        {
            if (_vsPath == null)
            {
                List<string> visualStudios = ListVisuaStudios();
                if (visualStudios.Count == 0)
                    return (false);
                _vsPath = visualStudios[0];
            }
            return (!string.IsNullOrEmpty(_vsPath));
        }
        public static bool IsConfiguratedCode()
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

        public static void OpenSolution(string solutionPath)
        {
            if (!IsConfiguratedVisualStudio())
                return;
            Process process = new Process();
            process.StartInfo.FileName = _vsPath;
            process.StartInfo.Arguments = solutionPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.Start();
        }

        public static void OpenCode(string workingDirectory)
        {
            if (!IsConfiguratedCode())
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

        public static List<string> ListVisuaStudios()
        {
            List<string> visualStudios = new List<string>();
            string path = @"C:\Program Files (x86)\Microsoft Visual Studio";
            string[] visualStudioPaths = Directory.GetDirectories(path);
            int? versionMajor = null;
            string pathMajor = null;
            foreach (string visualStudioPath in visualStudioPaths)
            {
                DirectoryInfo directory = new DirectoryInfo(visualStudioPath);
                if (!Int32.TryParse(directory.Name, out int version))
                    continue;
                if ((versionMajor.HasValue) && (versionMajor.Value > version))
                    continue;
                versionMajor = version;
                pathMajor = visualStudioPath;
            }
            if (pathMajor == null)
                return (visualStudios);
            foreach (string edition in Directory.GetDirectories(pathMajor))
            {
                string pathVS = $@"{edition}\Common7\IDE\devenv.exe";
                if (File.Exists(pathVS))
                    visualStudios.Add(pathVS);
            }
            return (visualStudios);
        }

        public static string Execute(string command, string arguments = null, string workingDirectory = null)
        {
            string fullCommand = $"{command} {arguments} ({workingDirectory})";
            WindowManager.AddLogStart(fullCommand);
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
            WindowManager.AddLogEnd(fullCommand);
            return (data);
        }
    }
}
