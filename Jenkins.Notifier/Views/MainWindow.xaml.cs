namespace Jenkins.Notifier.Views
{
    using System.ComponentModel;
    using System.Windows;

    using Jenkins.Notifier.ViewModel;

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Closing += this.OnClosing;
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            ViewModelLocator.Cleanup();
        }
    }
}