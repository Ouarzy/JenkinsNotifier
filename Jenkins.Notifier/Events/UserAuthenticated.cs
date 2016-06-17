namespace Jenkins.Notifier.Events
{
    using System.Collections.Generic;
    using System.Linq;

    using Jenkins.Notifier.Model;

    public class UserAuthenticated : IDomainEvent
    {
        public UserAuthenticated(string serverUrl, IList<JenkinsViewModel> jenkinsViews, string login, string apiToken, int refreshDelay)
        {
            this.ApiToken = apiToken;
            this.RefreshDelay = refreshDelay;
            this.Login = login;
            this.JenkinsViews = jenkinsViews;
            this.ServerUrl = serverUrl;
        }
        
        public string ServerUrl { get; private set; }

        public IList<JenkinsViewModel> JenkinsViews { get; private set; }

        public string Login { get; private set; }

        public string ApiToken { get; private set; }

        public int RefreshDelay { get; private set; }

        public override bool Equals(object obj)
        {
            return this.Equals((UserAuthenticated)obj);
        }

        private bool Equals(UserAuthenticated other)
        {
            return string.Equals(this.ServerUrl, other.ServerUrl) &&
                    this.JenkinsViewsAreEqual(other) &&
                    string.Equals(this.Login, other.Login) &&
                    string.Equals(this.ApiToken, other.ApiToken) &&
                    this.RefreshDelay == other.RefreshDelay;
        }

        private bool JenkinsViewsAreEqual(UserAuthenticated other)
        {
            var viewsAndJobsAreEqual = this.JenkinsViews.Count() == other.JenkinsViews.Count();
            
            var thisList = this.JenkinsViews.ToList();
            var otherList = other.JenkinsViews.ToList();

            for (var viewIndex = 0; viewIndex < thisList.Count() && viewsAndJobsAreEqual; viewIndex++)
            {
                viewsAndJobsAreEqual = thisList[viewIndex].View.Equals(otherList[viewIndex].View) && thisList[viewIndex].Jobs.Count == otherList[viewIndex].Jobs.Count;
                for (var jobIndex = 0; jobIndex < thisList[viewIndex].Jobs.Count && viewsAndJobsAreEqual; jobIndex++)
                {
                    if (!string.Equals(thisList[viewIndex].Jobs[jobIndex], otherList[viewIndex].Jobs[jobIndex]))
                    {
                        viewsAndJobsAreEqual = false;
                    }
                }
            }

            return viewsAndJobsAreEqual;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.ServerUrl != null ? this.ServerUrl.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.JenkinsViews != null ? this.JenkinsViews.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Login != null ? this.Login.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.ApiToken != null ? this.ApiToken.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.RefreshDelay.GetHashCode();
                return hashCode;
            }
        }
    }
}
