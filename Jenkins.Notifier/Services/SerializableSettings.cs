namespace Jenkins.Notifier.Services
{
    using System;
    using System.Collections.Generic;

    using Jenkins.Notifier.Model;

    [Serializable]
    public struct SerializableSettings
    {
        public string UrlServer { get; set; }

        public List<JenkinsViewModel> JenkinsViews { get; set; }

        public string Login { get; set; }

        public string Apitoken { get; set; }

        public int RefreshDelay { get; set; }
    }
}