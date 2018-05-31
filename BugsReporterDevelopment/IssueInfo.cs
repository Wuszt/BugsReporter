using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugsReporterModel;

namespace BugsReporterDevelopment
{
    internal class IssueInfo : ViewModelBase
    {
        private bool m_checked = false;
        public bool Checked
        {
            get
            {
                return m_checked;
            }

            set
            {
                SetProperty(ref m_checked, value);
            }
        }

        public Issue Issue { get; set; }
        public FileInfo Attachment { get; set; }

        public string IsAttachmentAvailable
        {
            get
            {
                return (Attachment != null) ? "Yes" : "No";
            }
        }
    }
}
