using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace BugsReporterServer.Controllers
{
    public class FilesController : ApiController
    {
        private const string c_filesDirectory = @"D:/";
        // GET: api/Files
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Files/5
        public string Get(int id)
        {
            return "value";
        }

        // PUT: api/Files/5
        public void Put(int id, [FromBody]string value)
        {
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
