using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using BugsReporterModel;

namespace BugsReporterDevelopment
{


    internal static class Loader
    {
        private const string c_serverAddress = "http://localhost:18982/api/";

        public static List<IssueInfo> GetAllIssuesInfos()
        {
            var issues = GetAllIssues();

            List<IssueInfo> issuesInfos = new List<IssueInfo>();

            for(int i=0;i<issues.Count;++i)
            {
                issuesInfos.Add(new IssueInfo()
                {
                    Issue = issues[i],
                    Attachment = GetFileInfoForIssue(issues[i].ID)
                });
            }

            return issuesInfos;
        }

        private static List<Issue> GetAllIssues()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(c_serverAddress);

                var gettingTask = client.GetAsync("issues");

                gettingTask.Wait();

                var readingTask = gettingTask.Result.Content.ReadAsAsync<List<Issue>>();

                readingTask.Wait();

                List<Issue> issues = readingTask.Result;

                return issues;
            }
        }

        private static FileInfo GetFileInfoForIssue(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(c_serverAddress);

                var gettingTask = client.GetAsync("files/" + id);

                gettingTask.Wait();

                var readingTask = gettingTask.Result.Content.ReadAsAsync<FileInfo>();

                readingTask.Wait();

                return readingTask.Result;
            }
        }
    }
}
