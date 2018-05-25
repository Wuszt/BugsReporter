using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using BugsReporterModel;

namespace BugsReporterClient
{
    public class IssuesSender
    {
        private Uri m_serverURI = null;

        public IssuesSender(string serverAdress)
        {
            m_serverURI = new Uri(serverAdress);
        }

        public void SendBug(string stack, string userInfo, string title = null, string description = null)
        {
            SendIssue(IssueType.Bug, stack, userInfo, title, description);
        }

        public void SendCrash(string stack, string userInfo)
        {
          //  SendIssue(IssueType.Crash, stack, userInfo, null);
        }

        private void SendIssue(IssueType type, string stack, string userInfo, string title, string desc )
        {
            Issue issue = new Issue()
            {
                Stack = stack,
                Type = type,
                UserInfo = userInfo,
                Title = title,
                Description = desc
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = m_serverURI;

                var postingTask = client.PostAsJsonAsync("issues", issue);

                postingTask.Wait();
            }
        }
    }
}
