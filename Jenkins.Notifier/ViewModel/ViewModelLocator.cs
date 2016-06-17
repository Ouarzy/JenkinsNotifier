namespace Jenkins.Notifier.ViewModel
{
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Jenkins.Notifier.Model;
    using Jenkins.Notifier.Services;

    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register(GetSettings);
            SimpleIoc.Default.Register(GetMessenger);
            SimpleIoc.Default.Register(GeJenkinServiceFactory);
            SimpleIoc.Default.Register(GetTimerFactory);
            SimpleIoc.Default.Register(GetUiService);
            SimpleIoc.Default.Register<MainViewModel>();
        }

        private static ITimerFactory GetTimerFactory()
        {
            return new TimerFactory();
        }

        private static ISettings GetSettings()
        {
            return new Settings(new FileSettings());
        }

        private static IMessenger GetMessenger()
        {
            return new Messenger();
        }

        private static IJenkinServiceFactory GeJenkinServiceFactory()
        {
            return new JenkinServiceFactory();
        }

        private static IUiService GetUiService()
        {
            return new UiService();
        }

        public static MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
        }
    }
}