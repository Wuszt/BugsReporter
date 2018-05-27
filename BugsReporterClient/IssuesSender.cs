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

        private Attachments m_attachments = null;

        public IssuesSender(string serverAdress, bool makeScreenShot, string[] customAttachments)
        {
            m_serverURI = new Uri(serverAdress);

            m_attachments = new Attachments(customAttachments, makeScreenShot);
        }

        public void UpdateCustomAttachments(string[] paths)
        {
            m_attachments.UpdateCustomFiles(paths);
        }

        public void ResetScreenShot(bool remake)
        {
            m_attachments.ResetScreenShot(remake);
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

            m_attachments.GetCompressedAttachments();

            using (var client = new HttpClient())
            {
                client.BaseAddress = m_serverURI;

                var postingTask = client.PostAsJsonAsync("issues", issue);

                postingTask.Wait();
            }
        }
    }
}
