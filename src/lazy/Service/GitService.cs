using lazy.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lazy.Service
{
    public class GitService
    {
        private const string ER_BRANCH = @"On branch (?<name>(\w+))";
        public static bool IsGitFolder(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
                if (directory.EndsWith(".git"))
                    return (true);
            return (false);
        }

        public static void Update(SolutionVO solution)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                Update(repository);
        }

        public static void Update(RepositoryVO repository)
        {
            string response = ConsoleService.Execute("git", "status", repository.Path);
            Match match = Regex.Match(response, ER_BRANCH);
            repository.Branch = match.Groups["name"].Value;
        }
    }
}
