namespace Jenkins.Notifier.ViewModel
{
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Model;

    public class JenkinsJobViewModel
    {
        public JenkinsJobViewModel(string displayName, JobStatus status, string url, int lastRanBuildNumber, ICommand runBuildCommand, ICommand stopbuildCommand, bool buildRunning = false)
        {
            this.DisplayName = displayName;
            this.Status = status;
            this.Url = url;
            this.LastRanBuildNumber = lastRanBuildNumber;
            this.BuildRunning = buildRunning;
            this.ShowJobOnline = new RelayCommand(this.OnShowJobOnline);
            this.RunBuildOnline = runBuildCommand;
            this.StopBuildOnline = stopbuildCommand;
        }

        public string Url { get; private set; }

        public string DisplayName { get; private set; }

        public ICommand ShowJobOnline { get; private set; }

        public ICommand RunBuildOnline { get; private set; }
     
        public ICommand StopBuildOnline { get; private set; }

        public JobStatus Status { get; private set; }

        public int LastRanBuildNumber { get; private set; }

        public bool BuildRunning { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is JenkinsJobViewModel && this.Equals((JenkinsJobViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.Url != null ? this.Url.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (int)this.Status;
                hashCode = (hashCode * 397) ^ (this.DisplayName != null ? this.DisplayName.GetHashCode() : 0);
                return hashCode;
            }
        }

        private void OnShowJobOnline()
        {
            System.Diagnostics.Process.Start(this.Url);
        }

        private bool Equals(JenkinsJobViewModel other)
        {
            return string.Equals(this.Url, other.Url) && this.Status == other.Status
                   && string.Equals(this.DisplayName, other.DisplayName);
        }
    }
}