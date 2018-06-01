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
        public MainWindow()
        {
            InitializeComponent();

            var model = new MainViewModel();
            model.Issues = Loader.GetAllIssuesInfos();
            listView.ItemsSource = model.IssuesView;
        }
    }
}
