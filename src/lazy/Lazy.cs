using lazy.VO;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy
{
    public class Lazy
    {
        public SolutionVO Solution { set; get; }

        public void RefreshSolution(Window window)
        {
            //window.RemoveAll();
            if (this.Solution == null)
                return;
            int x = 1;
            int y = 0;
            List<View> views = new List<View>();
            views.Add(new CheckBox(x, y++, this.Solution.Name));
            foreach(RepositoryVO repository in this.Solution.Repositories)
            {
                views.Add(new CheckBox(x + 4, y++, repository.Name));
                views.Add(new Label(x + 8, y++, GetProjectNames(repository.Projects)));
                views.Add(new Label(x + 8, y++, GetBranchInfo(repository)));
            }
            window.Add(views.ToArray());
        }

        private string GetBranchInfo(RepositoryVO repository)
        {
            StringBuilder builder = new StringBuilder("Branch ( ");
            builder.Append(repository.Branch);
            builder.Append(" )");
            return (builder.ToString());
        }

        private string GetProjectNames(List<ProjectVO> projects)
        {
            StringBuilder builder = new StringBuilder("Projects ( ");
            for (int i = 0; i < projects.Count; i++)
            {
                if (i > 0)
                    builder.Append(" , ");
                builder.Append(projects[i].Name);
            }
            builder.Append(" )");
            return (builder.ToString());
        }
    }
}
