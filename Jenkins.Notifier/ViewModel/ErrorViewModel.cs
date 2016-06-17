namespace Jenkins.Notifier.ViewModel
{
    using System;
    using System.Text;

    public class ErrorViewModel : BuildJenkinsViewModelBase
    {
        private string errorMessage;

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.SetProperty(ref this.errorMessage, value); }
        }

        public ErrorViewModel(string errorMessage, Exception exception)
        {
            var errorMessageLocal = new StringBuilder();
            errorMessageLocal.AppendLine(errorMessage);
            errorMessageLocal.AppendLine(exception.Message);

            this.errorMessage = errorMessageLocal.ToString();
        }
    }
}
