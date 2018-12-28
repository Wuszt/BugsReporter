using System;
using System.Linq;
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

        public string IsAttachmentAvailableAsString
        {
            get
            {
                return IsAttachmentAvailable ? "Yes" : "No";
            }
        }

        public bool IsAttachmentAvailable
        {
            get
            {
                return Attachment != null && Attachment.AttachmentsNames.Length > 0;
            }
        }

        public string AttachmentsList
        {
            get
            {
                if (!IsAttachmentAvailable)
                    return "";

                return Attachment.AttachmentsNames.Aggregate((x, y) => x + "\n" + y);
            }
        }

        public string ShortStack
        {
            get
            {
                int index = Issue.Stack.IndexOf(Environment.NewLine);

                if (index > -1)
                    return Issue.Stack.Substring(0, index);

                return Issue.Stack;
            }
        }
    }
}
