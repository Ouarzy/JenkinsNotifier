using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Jenkins.Core;

namespace Jenkins.Notifier.ViewModel.Commands
{
    public class RunBuildCommand : ICommand
    {
        private readonly IJenkinsRestClient jenkinsRestClient;
        private readonly string buildUrl;

        private bool executed;

        public RunBuildCommand(IJenkinsRestClient jenkinsRestClient, string buildUrl)
        {
            this.jenkinsRestClient = jenkinsRestClient;
            this.buildUrl = buildUrl;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(this.buildUrl) && !this.executed;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                this.executed = true;
                this.RaisedCanExecuteChanged();
                await this.jenkinsRestClient.RunJobAsync(this.buildUrl);
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
            return obj is RunBuildCommand && this.Equals((RunBuildCommand)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.jenkinsRestClient != null ? this.jenkinsRestClient.GetHashCode() : 0) * 397) ^ (this.buildUrl != null ? this.buildUrl.GetHashCode() : 0);
            }
        }

        private bool Equals(RunBuildCommand other)
        {
            return string.Equals(this.buildUrl, other.buildUrl);
        }
    }
}
