using lazy.VO;
using System;
using System.Collections.Generic;
using System.Text;

namespace lazy.Service
{
    public class AzureDevOpsService
    {
        private static string _vstsPath;
        public static bool IsConfigurated()
        {
            if (_vstsPath == null)
            {
                string response = ProcessService.Execute("where", "vsts");
                if (response.Contains("INFO:"))
                {
                    _vstsPath = "";
                }
                else
                {
                    _vstsPath = "";
                    string[] lines = response.Split("\r\n");
                    for (int i = lines.Length - 1; i >= 0; i--)
                    {
                        string line = lines[i].Trim();
                        if (string.IsNullOrEmpty(line))
                            continue;
                        _vstsPath = line;
                        break;
                    }
                }
            }
            return (!string.IsNullOrEmpty(_vstsPath));
        }
        public static List<WorkItemVO> ListWorkItems(string workingDirectory)
        {
            List<WorkItemVO> workItems = new List<WorkItemVO>();
            if (!IsConfigurated())
                return (null);
            string command = _vstsPath;
            string arguments = @"work item query --wiql ""SELECT System.ID, System.Title FROM workitems WHERE[System.AssignedTo] = @Me AND[System.State] = 'In Progress' ORDER BY System.ID DESC""";
            string response = ProcessService.Execute(command, arguments, workingDirectory);
            string[] lines = response.Split("\r\n");
            foreach (string line in lines)
            {
                WorkItemVO workItem = CreateWorkItem(line.Trim());
                if (workItem != null)
                    workItems.Add(workItem);
            }
            return (workItems);
        }

        private static WorkItemVO CreateWorkItem(string line)
        {
            if (string.IsNullOrEmpty(line))
                return (null);
            int index = line.IndexOf(" ");
            if (index < 0)
                return (null);
            if (!Int32.TryParse(line.Substring(0, index), out int id))
                return (null);
            WorkItemVO workItem = new WorkItemVO();
            workItem.Code = id;
            workItem.Name = line.Substring(index + 1).Trim();
            return (workItem);
        }

        public static List<PullRequestVO> ListPullRequests(string workingDirectory)
        {
            if (!IsConfigurated())
                return (null);
            List<PullRequestVO> pullRequests = new List<PullRequestVO>();
            string command = _vstsPath;
            string arguments = @"code pr list";
            string response = ProcessService.Execute(command, arguments, workingDirectory);
            TableVO table = new TableVO(response);
            foreach (TableRowVO row in table.Rows)
            {
                PullRequestVO pullRequest = new PullRequestVO();
                pullRequest.ID = Int32.Parse(row.GetValue("ID"));
                pullRequest.Title = row.GetValue("Title");
                pullRequest.Repository = row.GetValue("Repository");
                pullRequests.Add(pullRequest);
            }
            return (pullRequests);
        }

        public static void CreatePullRequest(RepositoryVO repository, WorkItemVO workItem)
        {
            //TODO: Work over here
        }
    }
}
