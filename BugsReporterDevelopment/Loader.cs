using System;
using System.Collections.Generic;
using System.Net.Http;
using BugsReporterModel;

namespace BugsReporterDevelopment
{
    internal static class Loader
    {
        private const string c_serverAddress = "http://localhost:18982/api/";

        public static string FilesPath
        {
            get
            {
                return c_serverAddress + "files/";
            }
        }

        public static List<IssueInfo> GetAllIssuesInfos()
        {
            var issues = GetAllIssues();

            var files = GetAllFileInfos();

            List<IssueInfo> issuesInfos = new List<IssueInfo>();

            for (int i = 0; i < issues.Count; ++i)
            {
                FileInfo file = null;

                files.TryGetValue(issues[i].ID, out file);

                issuesInfos.Add(new IssueInfo()
                {
                    Issue = issues[i],
                    Attachment = file
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

                return readingTask.Result;
            }
        }

        private static Dictionary<int, FileInfo> GetAllFileInfos()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(c_serverAddress);

                var gettingTask = client.GetAsync("files");

                gettingTask.Wait();

                var readingTask = gettingTask.Result.Content.ReadAsAsync<Dictionary<int, FileInfo>>();

                readingTask.Wait();

                return readingTask.Result;
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
