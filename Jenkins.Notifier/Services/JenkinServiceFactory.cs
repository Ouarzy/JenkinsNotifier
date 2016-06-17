namespace Jenkins.Notifier.Services
{
    using System.Collections.Generic;

    using Jenkins.Core;
    using Jenkins.Notifier.Model;

    public class JenkinServiceFactory : IJenkinServiceFactory
    {
        public IJenkinService Create(string serverUrl, IList<JenkinsViewModel> jenkinsViewModels, string userName, string apiToken)
        {
            var jenkinsRestFactory = new JenkinsRestFactory(userName, apiToken);
            return new JenkinsService(jenkinsRestFactory.GetClient(), serverUrl, jenkinsViewModels);
        }
    }
}