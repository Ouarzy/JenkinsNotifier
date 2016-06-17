namespace Jenkins.Notifier.Services
{
    using System.Collections.Generic;

    using Jenkins.Notifier.Model;

    public interface IJenkinServiceFactory
    {
        IJenkinService Create(string serverUrl, IList<JenkinsViewModel> jenkinsViewModels, string userName, string apiToken);
    }
}