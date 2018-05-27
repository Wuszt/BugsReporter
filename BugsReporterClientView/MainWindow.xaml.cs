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
        public MainWindow()
        {
            BugsReporterClient.Attachments att = new BugsReporterClient.Attachments(new string[] { "lol.txt" }, true);
            att.GetCompressedAttachments();
            InitializeComponent();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            BugsReporterClient.IssuesSender issueSender = new BugsReporterClient.IssuesSender("http://localhost:18982/api/issues");
            issueSender.SendBug("Error", MailTextBox.Text, TitleTextBox.Text, DescTextBox.Text);
            Application.Current.Shutdown();
        }
    }
}
