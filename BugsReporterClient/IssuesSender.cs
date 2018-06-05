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

        public void SendIssue(string stack, string userInfo, string title = null, string description = null )
        {
            Issue issue = new Issue()
            {
                Stack = stack,
                UserInfo = userInfo,
                Title = title,
                Description = description
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = m_serverURI;

                var postingTask = client.PostAsJsonAsync("issues", issue);

                postingTask.Wait();

                var readingTask = postingTask.Result.Content.ReadAsAsync<Issue>();

                readingTask.Wait();

                issue = readingTask.Result;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = m_serverURI;

                var postingTask = client.PostAsync("files/"+issue.ID, new ByteArrayContent(m_attachments.GetCompressedAttachments()));

                postingTask.Wait();
            }
        }
    }
}
