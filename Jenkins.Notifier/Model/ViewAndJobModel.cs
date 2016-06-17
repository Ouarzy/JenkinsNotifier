namespace Jenkins.Notifier.Model
{
    public class ViewAndJobModel
    {
        public ViewAndJobModel(string viewName, string jobName)
        {
            this.ViewName = viewName;
            this.JobName = jobName;
        }

        public string ViewName { get; private set; }

        public string JobName { get; private set; }
    }
}
