namespace Jenkins.Notifier.ViewModel
{
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Jenkins.Notifier.Views;

    public class NotifyIconViewModel
    {
        public NotifyIconViewModel()
        {
            this.ShowWindowCommand = new RelayCommand(this.OnShowWindowCommand, this.ShowWindowCanExecute);
            this.HideWindowCommand = new RelayCommand(this.OnHideWindowCommand, this.HideWindowCommandCanExecute);
            this.ExitApplicationCommand = new RelayCommand(this.OnExitApplicationCommand);
        }

        public ICommand ShowWindowCommand { get; private set; }

        public ICommand HideWindowCommand { get; private set; }

        public ICommand ExitApplicationCommand { get; private set; }

        private void OnExitApplicationCommand()
        {
            Application.Current.Shutdown();
        }

        private bool HideWindowCommandCanExecute()
        {
            return Application.Current.MainWindow != null;
        }

        private void OnHideWindowCommand()
        {
            Application.Current.MainWindow.Close();
        }

        private bool ShowWindowCanExecute()
        {
            return Application.Current.MainWindow == null;
        }

        private void OnShowWindowCommand()
        {
            Application.Current.MainWindow = new MainWindow();
            Application.Current.MainWindow.Show();
        }
    }
}
