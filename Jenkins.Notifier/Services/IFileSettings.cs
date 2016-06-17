namespace Jenkins.Notifier.Services
{
    using System.Collections.Generic;

    using Jenkins.Notifier.Model;

    public interface IFileSettings
    {
        void Save(string urlServer, IList<JenkinsViewModel> jenkinsViews, string login, string apitoken, int refreshDelay);

        SerializableSettings Load();
    }
}
