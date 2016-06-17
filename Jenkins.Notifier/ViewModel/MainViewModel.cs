namespace Jenkins.Notifier.ViewModel
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight.Messaging;

    using Jenkins.Notifier.Events;
    using Jenkins.Notifier.Model;
    using Jenkins.Notifier.Services;

    public class MainViewModel : BuildJenkinsViewModelBase
    {
        private readonly ISettings settings;

        private readonly IMessenger messenger;

        private readonly IUiService uiService;

        private readonly IJenkinServiceFactory jenkinServiceFactory;

        private readonly ITimerFactory timerFactory;

        private BuildJenkinsViewModelBase currentViewModel;

        public MainViewModel(ISettings settings, IMessenger messenger, IUiService uiService, IJenkinServiceFactory jenkinServiceFactory, ITimerFactory timerFactory)
        {
            this.settings = settings;
            this.messenger = messenger;
            this.uiService = uiService;
            this.jenkinServiceFactory = jenkinServiceFactory;
            this.timerFactory = timerFactory;
            messenger.Register<UserAuthenticated>(this, this.OnUserAuthenticated);
            messenger.Register<LoginViewRequired>(this, this.OnLoginViewRequired);
            messenger.Register<BuildOccured>(this, this.OnNotificationrequired);
        }

        public delegate void NotificationRequiredDelegate(object sender, NotificationRequiredArgs e);

        public delegate void LoginInfoRequiredDelegate(object sender, LoginInfoRequiredArgs e);

        public event NotificationRequiredDelegate NotificationRequired;

        public event LoginInfoRequiredDelegate LoginInfoRequired;
        
        public BuildJenkinsViewModelBase CurrentViewModel
        {
            get { return this.currentViewModel; }

            private set
            {
                if (this.currentViewModel != null)
                {
                    this.currentViewModel.Dispose();
                }

                this.SetProperty(ref this.currentViewModel, value);
            }
        }

        public void InitCurrentViewModel()
        {
            try
            {
                this.settings.Load();

                this.currentViewModel = new JenkinsViewsViewModel(this.messenger, this.jenkinServiceFactory.Create(this.settings.ServerUrl, this.settings.JenkinsViews, this.settings.Login, this.settings.ApiToken), this.timerFactory.Create(this.settings.RefreshDelay));
            }
            catch
            {
                this.currentViewModel = new LoginViewModel(this.settings, this.messenger);
                this.OnLoginInfoRequired(new LoginInfoRequiredArgs());
            }
        }

        private void OnLoginViewRequired(LoginViewRequired loginViewRequired)
        {
            this.CurrentViewModel = new LoginViewModel(this.settings, this.messenger)
                                   {
                                       ServerUrl = this.settings.ServerUrl,
                                       ViewsAndJobs = new ObservableCollection<ViewAndJobViewModel>(this.GetViewsAndJobs(this.settings.JenkinsViews)),
                                       Login = this.settings.Login,
                                       ApiToken = this.settings.ApiToken,
                                       RefreshDelay = this.settings.RefreshDelay
                                   };
        }

        private IEnumerable<ViewAndJobViewModel> GetViewsAndJobs(IEnumerable<JenkinsViewModel> jenkinsViewModels)
        {
            if (jenkinsViewModels == null)
            {
                return Enumerable.Empty<ViewAndJobViewModel>();
            }

            var viewModels = new List<ViewAndJobViewModel>();
            foreach (var jenkinsViewModel in jenkinsViewModels)
            {
                if (jenkinsViewModel.Jobs.Any())
                {
                    viewModels.AddRange(jenkinsViewModel.Jobs.Select(j => new ViewAndJobViewModel(this.messenger, new ViewAndJobModel(jenkinsViewModel.View, j))));
                }
                else
                {
                    viewModels.Add(new ViewAndJobViewModel(this.messenger, new ViewAndJobModel(jenkinsViewModel.View, string.Empty)));
                }
            }

            return viewModels;
        }

        private void OnUserAuthenticated(UserAuthenticated userAuthenticated)
        {
            var jenkinsService = this.jenkinServiceFactory.Create(userAuthenticated.ServerUrl, userAuthenticated.JenkinsViews, userAuthenticated.Login, userAuthenticated.ApiToken);
            this.CurrentViewModel = new JenkinsViewsViewModel(this.messenger, jenkinsService, new Timer(this.settings.RefreshDelay));
        }

        private void OnNotificationrequired(BuildOccured buildOccured)
        {
            this.OnNotificationrequired(new NotificationRequiredArgs(buildOccured));
        }

        private void OnNotificationrequired(NotificationRequiredArgs args)
        {
            var handler = this.NotificationRequired;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnLoginInfoRequired(LoginInfoRequiredArgs e)
        {
            var handler = this.LoginInfoRequired;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.currentViewModel != null)
            {
                this.currentViewModel.Dispose();
            }
        }
    }
}