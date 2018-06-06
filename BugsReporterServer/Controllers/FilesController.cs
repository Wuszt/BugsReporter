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

        private static SortedDictionary<int, FileInfo> m_cachedFilesInfos = new SortedDictionary<int, FileInfo>();

        // GET: api/Files
        public Dictionary<int,FileInfo> Get()
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

                Dictionary<int,FileInfo> files = new Dictionary<int, FileInfo>();

                foreach(var issue in issues)
                {
                    var file = Get(issue.ID);

                    if (file != null)
                        files.Add(issue.ID, file);
                }

                return files;
            }
        }

        // GET: api/Files/5
        public FileInfo Get(int id)
        {
            if(!m_cachedFilesInfos.ContainsKey(id))
            {
                if (!TryToLoadFileInfoToCache(id))
                    return null;
            }

            return m_cachedFilesInfos[id];
        }

        [Route("api/files/{id}/download")]
        public HttpResponseMessage GetFile(int id)
        {
            var dataBytes = File.ReadAllBytes(Path.Combine(c_filesDirectory, id + ".zip"));
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = id + ".zip";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }

        private bool TryToLoadFileInfoToCache(int id)
        {
            string filePath = Path.Combine(c_filesDirectory, id + ".zip");

            if (!File.Exists(filePath))
                return false;

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

            m_cachedFilesInfos[id] = result;

            return true;
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
