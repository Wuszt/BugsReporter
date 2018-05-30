using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using BugsReporterModel;
using FileInfo = BugsReporterModel.FileInfo;
using System.IO.Compression;
using Newtonsoft.Json;

namespace BugsReporterServer.Controllers
{
    public class FilesController : ApiController
    {
        private const string c_filesDirectory = @"D:/BugsLogger";
        // GET: api/Files
        public IEnumerable<FileInfo> Get()
        {
            using (var client = new HttpClient())
            {
                Issue[] issues = null;

                client.BaseAddress = new Uri("http://localhost:18982/api/");

                var gettingTask = client.GetAsync("issues");

                gettingTask.Wait();

                var readingTask = gettingTask.Result.Content.ReadAsStringAsync();

                readingTask.Wait();

                issues = JsonConvert.DeserializeObject<Issue[]>(readingTask.Result);

                List<FileInfo> files = new List<FileInfo>();

                foreach(var issue in issues)
                {
                    var file = Get(issue.ID);

                    if (file != null)
                        files.Add(file);
                }

                return files;
            }
        }

        // GET: api/Files/5
        public FileInfo Get(int id)
        {
            string filePath = Path.Combine(c_filesDirectory, id + ".zip");

            if (!File.Exists(filePath))
                return null;

            FileInfo result = null;

            using (ZipArchive archive = ZipFile.Open(Path.Combine(c_filesDirectory, id + ".zip"), ZipArchiveMode.Read))
            {
                result = new FileInfo()
                {
                    ArchiveName = filePath,
                    AttachmentsNames = archive.Entries.Select(x => x.FullName).ToArray(),
                    ArchiveSize = new System.IO.FileInfo(filePath).Length
                };
            }

            return result;
        }

        // DELETE: api/Files/5
        public void Delete(int id)
        {
        }

        // POST: api/Files
        public void Post(int id)
        {
            var reading = Request.Content.ReadAsStreamAsync();
            reading.Wait();

            using (Stream stream = reading.Result)
            {
                int length = (int)stream.Length;
                using (FileStream fileStream = File.Create(Path.Combine(c_filesDirectory, id + ".zip"), length))
                {
                    byte[] bytes = new byte[length];
                    stream.Read(bytes, 0, length);
                    fileStream.Write(bytes, 0, length);
                }
            }
        }
    }
}
