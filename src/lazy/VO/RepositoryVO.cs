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
        public bool Selected { set; get; }
        public int? Head { set; get; }
        public bool HasChanges
        {
            get
            {
                return (HasChangesNotStaged || HasChangesNotCommited || HasUnmergedPaths);
            }
        }
        public bool HasChangesNotStaged { set; get; }
        public bool HasChangesNotCommited { set; get; }
        public bool HasUnmergedPaths { set; get; }
        public bool HasChangesPush
        {
            get
            {
                return ((this.Head.HasValue) && (this.Head.Value > 0));
            }
        }
        public RepositoryVO()
        {
            this.Projects = new List<ProjectVO>();
        }
    }
}
