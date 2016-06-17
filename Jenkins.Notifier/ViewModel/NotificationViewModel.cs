namespace Jenkins.Notifier.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class NotificationViewModel : BuildJenkinsViewModelBase
    {
        private readonly ObservableCollection<JenkinsJobViewModel> jobs;

        public NotificationViewModel(IEnumerable<JenkinsJobViewModel> jobs)
        {
            this.jobs = new ObservableCollection<JenkinsJobViewModel>(jobs);
        }

        public IEnumerable<JenkinsJobViewModel> Jobs
        {
            get
            {
                return this.jobs;
            }
        }
    }
}
