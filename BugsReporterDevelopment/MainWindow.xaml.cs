using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BugsReporterDevelopment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel m_currentModel = null;

        public MainWindow()
        {
            InitializeComponent();
            m_currentModel = new MainViewModel();
            m_currentModel.Issues = Loader.GetAllIssuesInfos();
            DataContext = m_currentModel;
            m_currentModel.SelectedIssue = m_currentModel.Issues[0];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Loader.FilesPath + m_currentModel.SelectedIssue.Issue.ID + "/download");
        }
    }
}
