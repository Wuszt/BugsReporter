using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BugsReporterClientView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BugsReporterClient.IssuesSender m_issuesSender = null;
        private class AttachmentsListElement
        {
            public string FilePath = null;
            public CheckBox CheckBox = null;

            public AttachmentsListElement(ListBox parent, string filePath)
            {
                CheckBox = new CheckBox();
                parent.Items.Add(CheckBox);
                FilePath = filePath;
                CheckBox.Content = System.IO.Path.GetFileName(FilePath);
            }
        }

        private List<AttachmentsListElement> m_customAttachments = new List<AttachmentsListElement>();

        private CheckBox m_screenShotCheckBox = null;

        private class UsersInput
        {
            public string UsersContact { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        private UsersInput m_usersInput = new UsersInput();

        public MainWindow(string[] attachments, bool attachScreenshot)
        {
            InitializeComponent();
            this.DataContext = m_usersInput;
            InitializeAttachmentsList(attachments, attachScreenshot);
            m_issuesSender = new BugsReporterClient.IssuesSender("http://localhost:18982/api/", attachScreenshot, attachments);
        }

        private void InitializeAttachmentsList(string[] attachments, bool attachScreenShot)
        {
            if (attachScreenShot)
            {
                m_screenShotCheckBox = new CheckBox();
                listBox.Items.Add(m_screenShotCheckBox);
                m_screenShotCheckBox.Content = "Screenshot";
            }

            for (int i = 0; i < attachments.Length; ++i)
            {
                AddCustomAttachmentToList(attachments[i]);
            }
        }

        private void AddCustomAttachmentToList(string path)
        {
            m_customAttachments.Add(new AttachmentsListElement(listBox, path));
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_screenShotCheckBox.IsChecked == false)
                m_issuesSender.ResetScreenShot(false);

            m_issuesSender.UpdateCustomAttachments(m_customAttachments.Where(x => x.CheckBox.IsChecked == true).Select(y => y.FilePath).ToArray());

            m_issuesSender.SendIssue("Error", m_usersInput.UsersContact, m_usersInput.Title, m_usersInput.Description);
            Application.Current.Shutdown();
        }
    }
}
