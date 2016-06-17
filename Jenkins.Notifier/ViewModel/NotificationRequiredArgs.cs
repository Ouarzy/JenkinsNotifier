namespace Jenkins.Notifier.ViewModel
{
    using System;

    using Jenkins.Notifier.Events;

    public class NotificationRequiredArgs : EventArgs
    {
        public NotificationRequiredArgs(BuildOccured buildOccured)
        {
            this.BuildOccured = buildOccured;
        }

        public BuildOccured BuildOccured { get; private set; }
    }
}