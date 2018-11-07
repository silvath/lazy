using lazy.Service;
using lazy.Views;
using lazy.VO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Terminal.Gui;

namespace lazy
{
    public class Lazy
    {
        private const int COLOR_BACKGROUND = 159;
        private const int COLOR_NOT_STAGED = 200;
        private const int COLOR_NOT_COMMITTED = 32;
        private const int COLOR_UNMERGED_PATH = 0;
        private Window _windowStatus = null;
        public Lazy(Window windowStatus)
        {
            this.BranchBase = "master";
            this._windowStatus = windowStatus;
        }
        public SolutionVO Solution { set; get; }
        public WorkItemVO WorkItem { set; get; }
        public string BranchBase { set; get; }

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
            MessageBox.ErrorQuery(50, 7, "Changes", "There is repositories with changes or conflicts", "Ok");
            return (false);
        }

        public void CheckoutBranchGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!EnsureHasNoChanges())
                return;
            string defaultValue = string.Empty;
            if (this.WorkItem != null)
                defaultValue = this.WorkItem.TaskID;
            string branch = WindowManager.ShowDialogText("Choose the branch", "Branch:", defaultValue);
            if (string.IsNullOrEmpty(branch))
                return;
            GitService.CheckoutBranch(this.Solution, branch);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
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
            if (!this.Solution.HasChangesPush)
            {
                MessageBox.ErrorQuery(50, 7, "Commit", "There is no repositories to push", "Ok");
                return;
            }
            GitService.Push(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void AddGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!this.Solution.HasChangesNotStaged)
            {
                MessageBox.ErrorQuery(50, 7, "Add", "There is no files to add", "Ok");
                return;
            }
            GitService.Add(this.Solution);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void CommitGit()
        {
            if (this.Solution == null)
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!this.Solution.HasChangesNotCommited)
            {
                MessageBox.ErrorQuery(50, 7, "Commit", "There is no repositories to commit", "Ok");
                return;
            }
            string message = WindowManager.ShowDialogText("Commit", "Message");
            if (string.IsNullOrEmpty(message))
                return;
            GitService.Commit(this.Solution, message);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void ShowWorkItems()
        {
            if (this.Solution == null)
                return;
            if (!AzureDevOpsService.IsConfigurated())
            {
                MessageBox.ErrorQuery(50, 7, "VSTS", "You must configure VSTS client first", "Ok");
                return;
            }
            string path = Directory.GetParent(this.Solution.Path).FullName;
            List<WorkItemVO> workItems = AzureDevOpsService.ListWorkItems(path);
            Dictionary<WorkItemVO, string> objects = new Dictionary<WorkItemVO, string>();
            foreach (WorkItemVO workItem in workItems)
                objects.Add(workItem, string.Format("{0} - {1}", workItem.Code, workItem.Name));
            WorkItemVO workitemSelected = WindowManager.ShowDialogObjects<WorkItemVO>("WorkItems", objects);
            this.WorkItem = workitemSelected;
            this.RefreshUI();
        }

        public void CreateBranch()
        {
            if (this.Solution == null)
                return;
            if (this.WorkItem == null)
            {
                MessageBox.ErrorQuery(50, 7, "VSTS", "You must select a work item first", "Ok");
                return;
            }
            if (!EnsureHasNoChanges())
                return;
            GitService.CheckoutBranch(this.Solution, this.BranchBase);
            GitService.Pull(this.Solution);
            GitService.CreateBranch(this.Solution, this.WorkItem);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void CreatePullRequest()
        {
            if (this.Solution == null)
                return;
            //if (this.WorkItem == null)
            //{
            //    MessageBox.ErrorQuery(50, 7, "VSTS", "You must select a work item first", "Ok");
            //    return;
            //}
            if (!EnsureHasNoChanges())
                return;
            //GitService.CheckoutBranch(this.Solution, this.WorkItem.BranchName);
            //GitService.Pull(this.Solution);
            //GitService.UpdateStatus(this.Solution, true);
            if (!EnsureHasNoChanges())
                return;
            foreach (RepositoryVO repository in this.Solution.Repositories)
            {
                if ((!repository.Selected))
                    continue;
                List<PullRequestVO> pullRequests = AzureDevOpsService.ListPullRequests(repository.Path);

            }
            //TODO: Work over here
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void ShowBranchs(string repositoryName)
        {
            RepositoryVO repository = this.Solution.GetRepositoryByName(repositoryName);
            List<string> branchs = GitService.ListBranchs(repository);
            string branch = WindowManager.ShowDialogList("Branchs", branchs);
            if (string.IsNullOrEmpty(branch))
                return;
            GitService.UpdateStatus(this.Solution, true);
            if (!EnsureHasNoChanges())
                return;
            GitService.CheckoutBranch(this.Solution, branch);
            GitService.UpdateStatus(this.Solution, true);
            this.RefreshUI();
        }

        public void ShowFilesNotStaged(string repositoryName)
        {
            RepositoryVO repository = this.Solution.GetRepositoryByName(repositoryName);
            Dictionary<string, string> files = GitService.ListFilesNotStaged(repository);
            bool updated = WindowManager.ShowDialogFilesNotStaged("Files Not Staged", files);
            if (!updated)
                return;
            GitService.UpdateStatus(this.Solution);
            this.RefreshUI();
        }

        public void ShowFilesNotCommited(string repositoryName)
        {
            RepositoryVO repository = this.Solution.GetRepositoryByName(repositoryName);
            Dictionary<string, string> files = GitService.ListFilesNotCommited(repository);
            bool updated = WindowManager.ShowDialogFilesNotStaged("Files Not Commited", files);
            if (!updated)
                return;
            GitService.UpdateStatus(this.Solution);
            this.RefreshUI();
        }

        public void OpenCommandSolution(string repositoryName)
        {
            string path = Directory.GetParent(this.Solution.Path).FullName;
            ProcessService.Open(path);
        }

        public void OpenCommandRepository(string repositoryName)
        {
            RepositoryVO repository = this.Solution.GetRepositoryByName(repositoryName);
            ProcessService.Open(repository.Path);
        }

        public void RefreshUI()
        {
            Clear(_windowStatus);
            if (this.Solution == null)
                return;
            int x = 1;
            int y = 0;
            List<View> views = new List<View>();
            views.Add(CreateCheckbox(x, y++, this.Solution, this.WorkItem));
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

        private CheckBox CreateCheckbox(int x, int y, SolutionVO solution, WorkItemVO workitem)
        {
            string name = solution.Name;
            if (workitem != null)
                name = name + string.Format(" (WIT - {0} - {1} )", workitem.Code, workitem.Name);
            CheckBoxSolution checkbox = new CheckBoxSolution(this, x, y++, name, solution.Selected);
            checkbox.Toggled += Checkbox_Solution_Toggled;
            return (checkbox);
        }

        private CheckBox CreateCheckbox(int x, int y, RepositoryVO repository)
        {
            CheckBoxRepository checkbox = new CheckBoxRepository(this, x, y++, repository.Name, repository.Selected);
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
            if (repository.HasUnmergedPaths)
                views.Add(CreateLabel(x, y, " ", COLOR_UNMERGED_PATH));
            x = x + 2;
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
