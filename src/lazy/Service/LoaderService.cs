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
            if (!File.Exists(pathOrName))
                return (null);
            SolutionVO solution = new SolutionVO();
            solution.Name = Path.GetFileNameWithoutExtension(pathOrName);
            solution.Path = pathOrName;
            string solutionText = File.ReadAllText(pathOrName);
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
                RepositoryVO repository = solution.GetRepository(directoryRepository.FullName);
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
            GitService.Update(solution);
            return (solution);
        }
    }
}
