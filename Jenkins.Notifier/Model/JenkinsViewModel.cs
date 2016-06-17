namespace Jenkins.Notifier.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is a model for a Jenkins View
    /// And not a ViewModel from MVVM
    /// </summary>
    [Serializable]
    public class JenkinsViewModel
    {
        private JenkinsViewModel()
        {
        }

        public JenkinsViewModel(string view)
        {
            this.View = view;
            this.Jobs = new List<string>();            
        }

        public JenkinsViewModel(string view, List<string> jobs)
        {
            this.View = view;
            this.Jobs = jobs;
        }

        public string View { get; set; }

        public List<string> Jobs { get; set; }
    }
}
