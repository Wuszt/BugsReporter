﻿using System.Collections.Generic;
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

        private IssueInfo m_selectedIssue;

        public IssueInfo SelectedIssue
        {
            get
            {
                return m_selectedIssue;
            }

            set
            {
                SetProperty(ref m_selectedIssue, value);
            }
        }
    }
}
