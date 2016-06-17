namespace Jenkins.Notifier.Model
{
    using System.Collections.Generic;

    using Jenkins.Notifier.ViewModel;

    public class JenkinsViewViewModel
    {
        public JenkinsViewViewModel(string viewName, IEnumerable<JenkinsJobViewModel> jobs)
        {
            this.ViewName = viewName;
            this.Jobs = jobs;
        }
        public string ViewName { get; private set; }

        public IEnumerable<JenkinsJobViewModel> Jobs { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is JenkinsViewViewModel && this.Equals((JenkinsViewViewModel)obj);
        }

        private bool Equals(JenkinsViewViewModel other)
        {
            return string.Equals(this.ViewName, other.ViewName);
        }

        public override int GetHashCode()
        {
            return this.ViewName != null ? this.ViewName.GetHashCode() : 0;
        }
    }
}
