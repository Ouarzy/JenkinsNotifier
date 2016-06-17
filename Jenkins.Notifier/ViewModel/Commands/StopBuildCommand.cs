using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Jenkins.Core;

namespace Jenkins.Notifier.ViewModel.Commands
{
    public class StopBuildCommand : ICommand
    {
        private readonly string stopBuildUrl;
        private readonly IJenkinsRestClient jenkinsRestClient;

        private bool executed;

        public StopBuildCommand(IJenkinsRestClient jenkinsRestClient, string stopBuildUrl)
        {
            this.jenkinsRestClient = jenkinsRestClient;
            this.stopBuildUrl = stopBuildUrl;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(this.stopBuildUrl) && !this.executed;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                this.executed = true;
                this.RaisedCanExecuteChanged();
                await this.jenkinsRestClient.StopJobAsync(this.stopBuildUrl);
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        public async void Execute(object parameter)
        {
            await this.ExecuteAsync();
        }

        public event EventHandler CanExecuteChanged;

        private void RaisedCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is StopBuildCommand && this.Equals((StopBuildCommand)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.stopBuildUrl != null ? this.stopBuildUrl.GetHashCode() : 0) * 397) ^ (this.jenkinsRestClient != null ? this.jenkinsRestClient.GetHashCode() : 0);
            }
        }

        private bool Equals(StopBuildCommand other)
        {
            return string.Equals(this.stopBuildUrl, other.stopBuildUrl);
        }
    }
}
