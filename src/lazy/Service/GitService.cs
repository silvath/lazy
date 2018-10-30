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
        private const string ER_HASCHANGES_NOT_STAGED = "Changes not staged for commit";
        private const string ER_HASCHANGES_NOT_COMMITED = "Changes to be committed";
        private const string ER_HASCHANGES_UNTRACKED_FILES = "Untracked files:";
        public static bool IsGitFolder(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
                if (directory.EndsWith(".git"))
                    return (true);
            return (false);
        }

        public static void UpdateStatus(SolutionVO solution, bool onlySelected = false)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    UpdateStatus(repository);
        }

        public static void UpdateStatus(RepositoryVO repository)
        {
            string response = ConsoleService.Execute("git", "status", repository.Path);
            Match match = Regex.Match(response, ER_BRANCH);
            repository.Branch = match.Groups["name"].Value;
            repository.HasChangesNotStaged = Regex.IsMatch(response, ER_HASCHANGES_NOT_STAGED);
            if (!repository.HasChangesNotStaged)
                repository.HasChangesNotStaged = Regex.IsMatch(response, ER_HASCHANGES_UNTRACKED_FILES);
            repository.HasChangesNotCommited = Regex.IsMatch(response, ER_HASCHANGES_NOT_COMMITED);
        }

        public static void Pull(SolutionVO solution, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Pull(repository);
        }

        public static void Pull(RepositoryVO repository)
        {
            ConsoleService.Execute("git", "pull", repository.Path);
        }
        public static void Add(SolutionVO solution, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Add(repository);
        }

        public static void Add(RepositoryVO repository)
        {
            if(repository.HasChangesNotStaged)
                ConsoleService.Execute("git", "add .", repository.Path);
        }
    }
}
