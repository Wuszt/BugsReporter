using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsReporterModel
{
    public class FileInfo
    {
        public string ArchiveName { get; set; }

        public long ArchiveSize { get; set; }

        public string[] AttachmentsNames { get; set; }
    }
}
