using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class RepositoryVO
    {
        public string Name { set; get; }
        public string Path { set; get; }
        public string Branch { set; get; }
        public List<ProjectVO> Projects { set; get; }
        public RepositoryVO()
        {
            this.Projects = new List<ProjectVO>();
        }
    }
}
