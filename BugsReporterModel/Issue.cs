namespace BugsReporterModel
{
    public class Issue
    {
        public int ID { get; set; }
        public IssueType Type { get; set;}

        public string Stack { get; set; }

        public string UserInfo { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }

    public enum IssueType
    {
        Unknown = 0,
        Bug = 1,
        Crash = 2
    }
}
