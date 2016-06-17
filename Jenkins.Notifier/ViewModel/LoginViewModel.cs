namespace Jenkins.Notifier.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Jenkins.Notifier.Events;
    using Jenkins.Notifier.Properties;
    using Jenkins.Notifier.Model;

    public sealed class LoginViewModel : BuildJenkinsViewModelBase
    {
        private readonly ISettings settings;

        private readonly IMessenger messenger;

        private ErrorViewModel errorViewModel;

        private string login;

        private string serverUrl = "http://pic.sid.distribution.edf.fr/jenkins";

        private string apiToken;

        private ObservableCollection<ViewAndJobViewModel> viewsAndJobs = new ObservableCollection<ViewAndJobViewModel>();

        private string jobToAdd = string.Empty;

        private string viewToAdd;

        private int refreshDelay = 60;

        private bool isErrorDisplay;

        public LoginViewModel(ISettings settings, IMessenger messenger)
        {
            this.settings = settings;
            this.messenger = messenger;
            this.AddViewAndJobCommand = new RelayCommand(this.OnViewAdded, this.CanAddJob);
            this.LoginCommand = new RelayCommand(this.OnLoginCommand, this.CanConnect);
            messenger.Register<RemoveViewAndJobRequired>(this, this.OnRemoveJobRequired);
            this.IsErrorDisplay = false;
        }

        public string Login
        {
            get { return this.login; }

            set
            {
                if (this.SetProperty(ref this.login, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ApiToken
        {
            get { return this.apiToken; }

            set
            {
                if (this.SetProperty(ref this.apiToken, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ServerUrl
        {
            get { return this.serverUrl; }

            set
            {
                if (this.SetProperty(ref this.serverUrl, value))
                {
                    this.LoginCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string ViewToAdd
        {
            get { return this.viewToAdd; }

            set
            {
                if (this.SetProperty(ref this.viewToAdd, value))
                {
                    this.AddViewAndJobCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string JobToAdd
        {
            get { return this.jobToAdd; }

            set
            {
                if (this.SetProperty(ref this.jobToAdd, value))
                {
                    this.AddViewAndJobCommand.RaiseCanExecuteChanged();
                }
            }
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

        public ObservableCollection<ViewAndJobViewModel> ViewsAndJobs
        {
            get { return this.viewsAndJobs; }
            set { this.SetProperty(ref this.viewsAndJobs, value); }
        }

        public RelayCommand LoginCommand { get; private set; }

        public RelayCommand AddViewAndJobCommand { get; private set; }

        public int RefreshDelay
        {
            get { return this.refreshDelay; }
            set { this.SetProperty(ref this.refreshDelay, value); }
        }

        private bool CanAddJob()
        {
            return !string.IsNullOrEmpty(this.ViewToAdd);
        }

        private void OnViewAdded()
        {
            this.viewsAndJobs.Add(new ViewAndJobViewModel(this.messenger, new ViewAndJobModel(this.ViewToAdd, jobToAdd)));
        }

        private bool CanConnect()
        {
            return !string.IsNullOrEmpty(this.ServerUrl) && !string.IsNullOrEmpty(this.Login) && !string.IsNullOrEmpty(this.ApiToken);
        }

        private void OnLoginCommand()
        {
            try
            {
                var jenkinsViews = this.GetJenkinsViews(this.viewsAndJobs);

                this.settings.SetSettings(this.ServerUrl, jenkinsViews, this.Login, this.ApiToken, this.RefreshDelay);
                this.settings.Save();
                this.messenger.Send(new UserAuthenticated(this.ServerUrl, jenkinsViews, this.Login, this.ApiToken, this.RefreshDelay));
            }
            catch (Exception ex)
            {
                this.IsErrorDisplay = true;
                this.ErrorViewModel = new ErrorViewModel(Resources.SaveSettingsError, ex);
            }
        }

        private IList<JenkinsViewModel> GetJenkinsViews(IEnumerable<ViewAndJobViewModel> viewAndJobViewModels)
        {
            var jenkinsViews = new List<JenkinsViewModel>();
            foreach (var viewAndJobViewModel in viewAndJobViewModels)
            {
                var jenkinsView = jenkinsViews.FirstOrDefault(o => o.View.Equals(viewAndJobViewModel.ViewName));
                if (jenkinsView != null)
                {
                    jenkinsView.Jobs.Add(viewAndJobViewModel.JobName);
                }
                else
                {
                    var jobs = string.IsNullOrEmpty(viewAndJobViewModel.JobName) ? new List<string>() : new List<string> { viewAndJobViewModel.JobName };
                    jenkinsViews.Add(new JenkinsViewModel(viewAndJobViewModel.ViewName, jobs));
                }
            }

            return jenkinsViews;
        }

        private void OnRemoveJobRequired(RemoveViewAndJobRequired removeJobAndJobRequired)
        {
            var jobToRemove = this.ViewsAndJobs.FirstOrDefault(
                o => o.ViewName.Equals(removeJobAndJobRequired.ViewAndJobViewModel.ViewName) &&
                    o.JobName.Equals(removeJobAndJobRequired.ViewAndJobViewModel.JobName));
            if (jobToRemove != null)
            {
                this.ViewsAndJobs.Remove(jobToRemove);
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.messenger.Unregister<RemoveViewAndJobRequired>(this, this.OnRemoveJobRequired);
        }
    }
}