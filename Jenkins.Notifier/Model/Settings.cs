namespace Jenkins.Notifier.Model
{
    using System.Collections.Generic;

    using Jenkins.Notifier.Exceptions;
    using Jenkins.Notifier.Services;

    public class Settings : ISettings
    {
        private readonly IFileSettings fileSettings;

        public Settings(IFileSettings fileSettings)
        {
            this.fileSettings = fileSettings;
        }

        private bool AreSettingsSet
        {
            get
            {
                return !string.IsNullOrEmpty(this.ServerUrl) 
                    && !string.IsNullOrEmpty(this.Login)
                    && !string.IsNullOrEmpty(this.ApiToken) 
                    && this.JenkinsViews != null;
            }
        }

        public string ServerUrl { get; private set; }

        public IList<JenkinsViewModel> JenkinsViews { get; private set; }

        public string Login { get; private set; }

        public string ApiToken { get; private set; }

        public int RefreshDelay { get; private set; }

        public void SetSettings(string urlServer, IList<JenkinsViewModel> jobsByView, string login, string apitoken, int refreshDelay)
        {
            this.ServerUrl = urlServer;
            this.JenkinsViews = jobsByView;
            this.Login = login;
            this.ApiToken = apitoken;
            this.RefreshDelay = refreshDelay;
        }

        public void Load()
        {
            var settings = this.fileSettings.Load();
            this.ServerUrl = settings.UrlServer;
            this.JenkinsViews = settings.JenkinsViews;
            this.Login = settings.Login;
            this.ApiToken = settings.Apitoken;
            this.RefreshDelay = settings.RefreshDelay;
        }

        public void Save()
        {
            if (!this.AreSettingsSet)
            {
                throw new InvalidSettingsException();
            }

            this.fileSettings.Save(this.ServerUrl, this.JenkinsViews, this.Login, this.ApiToken, this.RefreshDelay);
        }
    }
}
