using lazy.Service;
using lazy.VO;
using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace lazy
{
    public class Lazy
    {
        private const int COLOR_BACKGROUND = 159;
        private const int COLOR_NOT_STAGED = 200;
        private const int COLOR_NOT_COMMITTED = 32;
        private Window _windowStatus = null;
        public Lazy(Window windowStatus)
        {
            this._windowStatus = windowStatus;
        }
        public SolutionVO Solution { set; get; }

        public void RefreshGitStatus()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution);
            this.RefreshUI();
        }

        public bool EnsureHasNoChanges()
        {
            if (!this.Solution.HasChanges)
                return (true);
            MessageBox.ErrorQuery(50, 7, "Changes", "There is repositoies with changes", "Ok");
            return (false);
        }

        public void CheckoutGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!EnsureHasNoChanges())
                return;

        }

        public void FetchGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            GitService.Fetch(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void PullGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!EnsureHasNoChanges())
                return;
            GitService.Pull(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void PushGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            GitService.Push(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void AddGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            GitService.Add(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void RefreshUI()
        {
            Clear(_windowStatus);
            if (this.Solution == null)
                return;
            int x = 1;
            int y = 0;
            List<View> views = new List<View>();
            views.Add(CreateCheckbox(x, y++, this.Solution));
            foreach (RepositoryVO repository in this.Solution.Repositories)
            {
                views.Add(CreateCheckbox(x + 4, y++, repository));
                views.Add(CreateLabel(x + 8, y++, GetProjectsName(repository)));
                views.Add(CreateLabel(x + 8, y++, GetBranchInfo(repository)));
                InsertRepositoryInfo(views, repository, x + 8, y++);
            }
            _windowStatus.Add(views.ToArray());
        }

        private void Clear(Window window)
        {
            while (window.Subviews[0].Subviews.Count > 0)
                window.Remove(window.Subviews[0].Subviews[window.Subviews[0].Subviews.Count - 1]);
        }

        private CheckBox CreateCheckbox(int x, int y, SolutionVO solution)
        {
            CheckBox checkbox = new CheckBox(x, y++, solution.Name, solution.Selected);
            checkbox.Toggled += Checkbox_Solution_Toggled;
            return (checkbox);
        }

        private CheckBox CreateCheckbox(int x, int y, RepositoryVO repository)
        {
            CheckBox checkbox = new CheckBox(x, y++, repository.Name, repository.Selected);
            checkbox.Toggled += Checkbox_Repository_Toggled;
            return (checkbox);
        }

        private Label CreateLabel(int x, int y, string text, int? color = null)
        {
            Label label = new Label(x, y, text);
            if (color.HasValue)
            {
                label.TextColor = new Terminal.Gui.Attribute(color.Value);
            }
            return (label);
        }

        private void InsertRepositoryInfo(List<View> views, RepositoryVO repository, int x, int y)
        {
            if (repository.HasChangesNotStaged)
                views.Add(CreateLabel(x, y, " ", COLOR_NOT_STAGED));
            x = x + 2;
            if (repository.HasChangesNotCommited)
                views.Add(CreateLabel(x, y, " ", COLOR_NOT_COMMITTED));
            x = x + 2;
            if (repository.Head.HasValue)
                views.Add(CreateLabel(x, y, repository.Head.Value.ToString("+0;-#")));
        }

        private void Checkbox_Solution_Toggled(object sender, EventArgs e)
        {
            this.Solution.Selected = ((CheckBox)sender).Checked;
            this.RefreshUI();
        }

        private void Checkbox_Repository_Toggled(object sender, EventArgs e)
        {
            CheckBox checkbox = ((CheckBox)sender);
            foreach (RepositoryVO repository in this.Solution.Repositories)
            {
                if (repository.Name != checkbox.Text)
                    continue;
                repository.Selected = ((CheckBox)sender).Checked;
                this.RefreshUI();
                break;
            }
        }

        private string GetBranchInfo(RepositoryVO repository)
        {
            StringBuilder builder = new StringBuilder("Branch ( ");
            builder.Append(repository.Branch);
            builder.Append(" )");
            return (builder.ToString());
        }

        private string GetProjectsName(RepositoryVO repository)
        {
            return (string.Format("Projects {0}", GetProjectNames(repository.Projects)));
        }

        private string GetProjectNames(List<ProjectVO> projects)
        {
            StringBuilder builder = new StringBuilder(" ( ");
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
