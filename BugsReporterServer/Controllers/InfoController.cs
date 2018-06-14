using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BugsReporterServer.Controllers
{
    public class InfoController : ApiController
    {
        [Route("status")]
        public List<string> GetStatus()
        {
            return StatusHelpers.GetCPUTemperatureData();
        }
    }
}
