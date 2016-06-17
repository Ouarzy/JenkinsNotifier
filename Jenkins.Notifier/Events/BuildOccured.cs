namespace Jenkins.Notifier.Events
{
    using System.Collections.Generic;
    using System.Linq;

    using Jenkins.Notifier.ViewModel;

    public class BuildOccured : IDomainEvent
    {
        public BuildOccured(IEnumerable<JenkinsJobViewModel> jobs)
        {
            this.Jobs = jobs;
        }

        public IEnumerable<JenkinsJobViewModel> Jobs { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is BuildOccured && this.Equals((BuildOccured)obj);
        }

        private bool Equals(BuildOccured other)
        {
            return this.Jobs.All(o => other.Jobs.Select(u => u).Contains(o));
        }

        public override int GetHashCode()
        {
            return this.Jobs != null ? this.Jobs.GetHashCode() : 0;
        }
    }
}