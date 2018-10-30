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
        private const string ER_BRANCH_AHREAD = @"Your branch is ahead of \'(\w|\/)+\' by (?<head>(\d)+) commit";
        private const string ER_BRANCH_BEHIND = @"Your branch is behind \'(\w|\/)+\' by (?<head>(\d)+) commit";
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
            string response = ProcessService.Execute("git", "status", repository.Path);
            Match match = Regex.Match(response, ER_BRANCH);
            repository.Branch = match.Groups["name"].Value;
            repository.HasChangesNotStaged = Regex.IsMatch(response, ER_HASCHANGES_NOT_STAGED);
            if (!repository.HasChangesNotStaged)
                repository.HasChangesNotStaged = Regex.IsMatch(response, ER_HASCHANGES_UNTRACKED_FILES);
            repository.Head = null;
            match = Regex.Match(response, ER_BRANCH_AHREAD);
            if ((match != null) && (!string.IsNullOrEmpty(match.Groups["head"].Value)))
                repository.Head = Int32.Parse(match.Groups["head"].Value);
            match = Regex.Match(response, ER_BRANCH_BEHIND);
            if ((match != null) && (!string.IsNullOrEmpty(match.Groups["head"].Value)))
                repository.Head = 0 - Int32.Parse(match.Groups["head"].Value);
            repository.HasChangesNotCommited = Regex.IsMatch(response, ER_HASCHANGES_NOT_COMMITED);
        }

        public static void Fetch(SolutionVO solution, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Fetch(repository);
        }

        public static void Fetch(RepositoryVO repository)
        {
            ProcessService.Execute("git", "fetch", repository.Path);
        }

        public static void Pull(SolutionVO solution, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Pull(repository);
        }

        public static void Pull(RepositoryVO repository)
        {
            ProcessService.Execute("git", "pull", repository.Path);
        }

        public static void Push(SolutionVO solution, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Push(repository);
        }

        public static void Push(RepositoryVO repository)
        {
            if ((!repository.Head.HasValue) || (repository.Head.Value < 0))
                return;
            ProcessService.Execute("git", "push", repository.Path);
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
                ProcessService.Execute("git", "add .", repository.Path);
        }

        public static void Commit(SolutionVO solution, string message, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    Commit(repository, message);
        }

        public static void Commit(RepositoryVO repository, string message)
        {
            if (repository.HasChangesNotCommited)
                ProcessService.Execute("git", string.Format(@"commit -m ""{0}""", message), repository.Path);
        }

        public static void CheckoutBranch(SolutionVO solution, string branch, bool onlySelected = true)
        {
            foreach (RepositoryVO repository in solution.Repositories)
                if ((!onlySelected) || (repository.Selected))
                    CheckoutBranch(repository, branch);
        }

        public static void CheckoutBranch(RepositoryVO repository, string branch)
        {
            List<string> branchs = ListBranchs(repository);
            if (branchs.Contains(branch))
                ProcessService.Execute("git", string.Format(@"checkout {0}", branch), repository.Path);
        }

        public static List<string> ListBranchs(RepositoryVO repository)
        {
            List<string> branchs = new List<string>();
            string response = ProcessService.Execute("git", "branch --list", repository.Path);
            string[] lines = response.Split(System.Environment.NewLine);
            foreach (string line in lines)
            {

            }
            return (branchs);
        }
    }
}
