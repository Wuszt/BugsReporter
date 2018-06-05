namespace BugsReporterModel
{
    public class Issue
    {
        public int ID { get; set; }

        public string Stack { get; set; }

        public string UserInfo { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool AttachedScreenshot { get; set; }

        public string[] CustomAttachmentsFiles { get; set; }
    }
}
