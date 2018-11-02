using lazy.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace lazy.Service
{
    public class LoaderService
    {
        private const string ER_SOLUTION_PROJECT = @"Project\(\""\{(\w|\d|\-)+\}""\) = \""(?<name>(\w)+)\""\, \""(?<path>(\w|\.|\\)+)""";
        public static SolutionVO Load(string pathOrName)
        {
            string path = GetPath(pathOrName);
            if (path == null)
                return (null);
            SolutionVO solution = new SolutionVO();
            solution.Name = Path.GetFileNameWithoutExtension(path);
            solution.Path = path;
            string solutionText = File.ReadAllText(path);
            foreach (Match match in Regex.Matches(solutionText, ER_SOLUTION_PROJECT))
            {
                //Project
                ProjectVO project = new ProjectVO();
                project.Name = match.Groups["name"].Value;
                project.Path = match.Groups["path"].Value;
                solution.Projects.Add(project);
                //Repository
                FileInfo fileSolution = new FileInfo(solution.Path);
                FileInfo fileInfo = new FileInfo(Path.Combine(fileSolution.Directory.FullName, project.Path));
                DirectoryInfo directoryRepository = fileInfo.Directory;
                while ((directoryRepository != null) && (!GitService.IsGitFolder(directoryRepository.FullName)))
                    directoryRepository = directoryRepository.Parent;
                if (directoryRepository == null)
                    continue;
                RepositoryVO repository = solution.GetRepositoryByPath(directoryRepository.FullName);
                if (repository == null)
                {
                    repository = new RepositoryVO();
                    repository.Name = directoryRepository.Name;
                    repository.Path = directoryRepository.FullName;
                    solution.Repositories.Add(repository);
                }
                project.Repository = repository;
                repository.Projects.Add(project);
            }
            GitService.UpdateStatus(solution);
            return (solution);
        }

        private static string GetPath(string pathOrName)
        {
            if (File.Exists(pathOrName))
                return (pathOrName);
            string directory = Directory.GetCurrentDirectory();
            string pathCombine = Path.Combine(directory, pathOrName);
            if (File.Exists(pathCombine))
                return (pathCombine);
            foreach (string file in Directory.GetFiles(directory))
            {
                if (!file.EndsWith(".sln"))
                    continue;
                if (file.ToLower().Contains(pathOrName.ToLower()))
                    return (file);
            }
            return (null);
        }
    }
}
