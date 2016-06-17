using GalaSoft.MvvmLight.Command;

namespace Jenkins.Notifier.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Messaging;

    using Properties;
    using Events;
    using Model;
    using Services;
    using Commands;

    public sealed class JenkinsViewsViewModel : BuildJenkinsViewModelBase
    {
        private readonly IMessenger messenger;

        private readonly IJenkinService jenkinService;

        private readonly ITimer refreshTimer;

        private ObservableCollection<JenkinsViewViewModel> jenkinsViews = new ObservableCollection<JenkinsViewViewModel>();

        private int selectedIndex;

        private IEnumerable<JenkinsJobViewModel> oldFailingJobs;

        private DateTime lastRefresh;

        private ErrorViewModel errorViewModel;

        private bool isErrorDisplay;

        private bool isLoading;

        public JenkinsViewsViewModel(IMessenger messenger, IJenkinService jenkinService, ITimer refreshTimer)
        {
            this.messenger = messenger;
            this.jenkinService = jenkinService;
            this.refreshTimer = refreshTimer;
            this.RefreshJobs = new AwaitableDelegateCommand(this.OnRefreshJobs);
            this.Previous = new RelayCommand(this.OnPreviousRequired);
            this.refreshTimer.Tick += this.RefreshTimerTick;
            this.IsErrorDisplay = false;
        }

        public ErrorViewModel ErrorViewModel
        {
            get { return this.errorViewModel; }
            set { this.SetProperty(ref this.errorViewModel, value); }
        }

        public bool IsErrorDisplay
        {
            get { return this.isErrorDisplay; }
            private set { this.SetProperty(ref this.isErrorDisplay, value); }
        }

        public bool IsLoading
        {
            get { return this.isLoading; }
            private set { this.SetProperty(ref this.isLoading, value); }
        }

        public ObservableCollection<JenkinsViewViewModel> JenkinsViews
        {
            get { return this.jenkinsViews; }
            set { this.SetProperty(ref this.jenkinsViews, value); }
        }

        public int SelectedIndex
        {
            get { return this.selectedIndex; }
            set { this.SetProperty(ref this.selectedIndex, value); }
        }

        public DateTime LastRefresh
        {
            get { return this.lastRefresh; }
            set { this.SetProperty(ref this.lastRefresh, value); }
        }

        public IAsyncCommand RefreshJobs { get; private set; }

        public ICommand Previous { get; private set; }

        private void OnPreviousRequired()
        {
            this.messenger.Send(new LoginViewRequired(this));
        }

        private async void RefreshTimerTick(object sender, TimerEventArgs e)
        {
            await this.OnRefreshJobs();
        }

        private async Task OnRefreshJobs()
        {
            try
            {
                this.IsLoading = true;

                await this.jenkinService.Views().ContinueWith(
                    task =>
                    {
                        this.JenkinsViews = new ObservableCollection<JenkinsViewViewModel>(task.Result.Select(o => new JenkinsViewViewModel(o.ViewName, o.Jobs)));
                        this.NotifyIfRequired();
                        this.LastRefresh = DateTime.Now;
                    });

                this.IsErrorDisplay = false;

                if (this.ErrorViewModel != null)
                {
                    this.ErrorViewModel.Dispose();
                }
                this.ErrorViewModel = null;
            }
            catch (Exception ex)
            {
                this.IsErrorDisplay = true;
                this.ErrorViewModel = new ErrorViewModel(Resources.JenkinsServiceError, ex);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void NotifyIfRequired()
        {
            var failingJobs = this.GetJobsInStatus(JobStatus.Failed).ToList();

            var jobsThatChangedStatus = this.JobsToNotifySincePreviousRefresh(failingJobs);
            if (jobsThatChangedStatus.Any())
            {
                this.messenger.Send(new BuildOccured(jobsThatChangedStatus));
            }
        }

        private IList<JenkinsJobViewModel> JobsToNotifySincePreviousRefresh(IList<JenkinsJobViewModel> failingJobs)
        {
            var jobsThatChangeStatus = this.oldFailingJobs != null ? this.GetDifferencesBetweenFailingJobsAndOldFailingJobs(failingJobs) : failingJobs;
            jobsThatChangeStatus = this.GetAllJobsViewModels().Where(o => jobsThatChangeStatus.Select(u => u.Url).Contains(o.Url)).ToList();
            this.oldFailingJobs = failingJobs;

            return jobsThatChangeStatus;
        }

        private IEnumerable<JenkinsJobViewModel> GetAllJobsViewModels()
        {
            return from jenkinsView in this.jenkinsViews from job in jenkinsView.Jobs select job;
        }

        private List<JenkinsJobViewModel> GetDifferencesBetweenFailingJobsAndOldFailingJobs(ICollection<JenkinsJobViewModel> failingJobs)
        {
            var jobsThatChangeStatus = failingJobs.Count > this.oldFailingJobs.Count()
                                ? failingJobs.Where(o => !this.oldFailingJobs.Select(u => u.Url).Contains(o.Url)).ToList()
                                : this.oldFailingJobs.Where(o => !failingJobs.Select(u => u.Url).Contains(o.Url)).ToList();

            var failingJobsWithNewBuilds = failingJobs
                    .Where(o => this.oldFailingJobs.Any(old => old.Url == o.Url && old.LastRanBuildNumber != o.LastRanBuildNumber))
                    .ToList();

            return jobsThatChangeStatus.Concat(failingJobsWithNewBuilds).Distinct().ToList();
        }

        private IEnumerable<JenkinsJobViewModel> GetJobsInStatus(JobStatus jobStatus)
        {
            return this.GetAllJobsViewModels().Where(o => o.Status == jobStatus);
        }

        protected override void Dispose(bool disposing)
        {
            this.refreshTimer.Dispose();
        }
    }
}
