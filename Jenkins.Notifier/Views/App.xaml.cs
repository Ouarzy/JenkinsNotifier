using System.Threading;

namespace Jenkins.Notifier.Views
{
    using System.Windows;
    using System.Windows.Controls.Primitives;

    using GalaSoft.MvvmLight.Threading;

    using Hardcodet.Wpf.TaskbarNotification;

    using Jenkins.Notifier.ViewModel;

    public partial class App
    {
        private TaskbarIcon notifyIcon;

        static App()
        {
            Thread.CurrentThread.Name = "JenkinsNotifierAppUiThread";
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.notifyIcon = (TaskbarIcon)this.FindResource("NotifyIcon");

            ViewModelLocator.Main.NotificationRequired += this.OnNotificationRequired;
            ViewModelLocator.Main.LoginInfoRequired += this.OnLoginInfoRequired;
            ViewModelLocator.Main.InitCurrentViewModel();
        }

        private void OnNotificationRequired(object sender, NotificationRequiredArgs args)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () => this.notifyIcon.ShowCustomBalloon(
                    new NotificationBuild
                        {
                            DataContext = new NotificationViewModel(args.BuildOccured.Jobs)
                        },
                    PopupAnimation.Slide,
                    10000));
        }

        private void OnLoginInfoRequired(object sender, LoginInfoRequiredArgs args)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(
                () =>
                    {
                        Current.MainWindow = new MainWindow();
                        Current.MainWindow.Show();
                    });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ViewModelLocator.Main.NotificationRequired -= this.OnNotificationRequired;
            ViewModelLocator.Main.LoginInfoRequired -= this.OnLoginInfoRequired;
            this.notifyIcon.Dispose();
            base.OnExit(e);
        }
    }
}
