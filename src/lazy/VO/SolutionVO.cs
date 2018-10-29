using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class SolutionVO
    {
        public string Name { set; get; }
        public string Path { set; get; }
        public List<ProjectVO> Projects { set; get; }
        public List<RepositoryVO> Repositories { set; get; }
        public SolutionVO()
        {
            this.Projects = new List<ProjectVO>();
            this.Repositories = new List<RepositoryVO>();
        }

        public RepositoryVO GetRepository(string path)
        {
            foreach (RepositoryVO repository in this.Repositories)
                if (repository.Path == path)
                    return (repository);
            return (null);
        }
    }
}
