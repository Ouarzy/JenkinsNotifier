namespace Jenkins.Notifier.Events
{
    using Jenkins.Notifier.ViewModel;

    public class RemoveViewAndJobRequired : IDomainEvent
    {
        public ViewAndJobViewModel ViewAndJobViewModel { get; private set; }

        public RemoveViewAndJobRequired(ViewAndJobViewModel viewAndJobViewModel)
        {
            this.ViewAndJobViewModel = viewAndJobViewModel;
        }

        public override bool Equals(object obj)
        {
            return obj is RemoveViewAndJobRequired && this.Equals((RemoveViewAndJobRequired)obj);
        }

        private bool Equals(RemoveViewAndJobRequired other)
        {
            return string.Equals(this.ViewAndJobViewModel.ViewName, other.ViewAndJobViewModel.ViewName)
                   && string.Equals(this.ViewAndJobViewModel.JobName, other.ViewAndJobViewModel.JobName);
        }

        public override int GetHashCode()
        {
            return this.ViewAndJobViewModel != null ? this.ViewAndJobViewModel.GetHashCode() : 0;
        }
    }
}
