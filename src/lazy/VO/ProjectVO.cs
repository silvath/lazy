using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.VO
{
    public class ProjectVO
    {
        public string Name { set; get; }
        public string Path { set; get; }
        public RepositoryVO Repository { set; get; }
    }
}
