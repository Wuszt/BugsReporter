using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BugsReporterDevelopment
{
    internal class MainViewModel : ViewModelBase
    {
        private List<IssueInfo> m_issues;
        public List<IssueInfo> Issues
        {
            get
            {
                return m_issues;
            }
            set
            {
                if (SetProperty(ref m_issues, value))
                {
                    IssuesView = new ListCollectionView(value);
                }
            }
        }

        private ListCollectionView m_issuesView;
        public ListCollectionView IssuesView
        {
            get
            {
                return m_issuesView;
            }
            set
            {
                SetProperty(ref m_issuesView, value);
            }
        }
    }
}
