namespace Jenkins.Notifier.Model
{
    using System.Collections.Generic;

    public interface ISettings
    {
        string ServerUrl { get; }

        IList<JenkinsViewModel> JenkinsViews { get; }

        string Login { get; }

        string ApiToken { get; }

        int RefreshDelay { get; }

        void SetSettings(string urlServer, IList<JenkinsViewModel> jobsByView, string login, string apitoken, int refreshDelay);

        void Load();

        void Save();
    }
}