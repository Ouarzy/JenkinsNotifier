namespace Jenkins.Notifier.ViewModel
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    using GalaSoft.MvvmLight.Messaging;

    using Jenkins.Notifier.Events;
    using Jenkins.Notifier.Model;

    public class ViewAndJobViewModel : BuildJenkinsViewModelBase
    {
        private readonly IMessenger messenger;

        private string viewName;

        private string jobName;

        public ViewAndJobViewModel(IMessenger messenger, ViewAndJobModel viewAndJobModel)
        {
            this.messenger = messenger;
            this.viewName = viewAndJobModel.ViewName;
            this.jobName = viewAndJobModel.JobName;
            this.DeleteCommand = new RelayCommand(() => this.messenger.Send(new RemoveViewAndJobRequired(this)));
        }

        public string ViewName
        {
            get { return this.viewName; }
            set { this.SetProperty(ref this.viewName, value); }
        }

        public string JobName
        {
            get { return this.jobName; }
            private set { this.SetProperty(ref this.jobName, value); }
        }

        public ICommand DeleteCommand { get; private set; }
    }
}