using Jenkins.Core;
using Jenkins.Notifier.ViewModel.Commands;

namespace Jenkins.Notifier.Tests.ViewModel
{
    using System.Collections.Generic;

    using GalaSoft.MvvmLight.Messaging;

    using Events;
    using Notifier.Model;
    using Notifier.Services;
    using Notifier.ViewModel;

    using Moq;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class MainViewModelTest
    {
        private bool notificationBuildWasRequired;

        private bool loginInfoWasRequiredWasRequired;

        private readonly JenkinsJobViewModel jenkinsJobViewModel = new JenkinsJobViewModel("job1", JobStatus.Failed, string.Empty, 1265, new RunBuildCommand(new Mock<IJenkinsRestClient>().Object, "BuildUrl1"), new StopBuildCommand(new Mock<IJenkinsRestClient>().Object, "StopBuildUrl1"));

        [SetUp]
        public void BeforeTests()
        {
            this.notificationBuildWasRequired = false;
        }

        [Test]
        public void WhenInitializedThenSettingIsLoadAndCurrentViewModelIsLoginIfNoSettingsWereLoadedAndLoginInfoEventIsRaised()
        {
            var settings = new Mock<ISettings>();
            Check.That(this.loginInfoWasRequiredWasRequired).IsFalse();
            var vm = new MainViewModel(settings.Object, new Mock<IMessenger>().Object, new Mock<IUiService>().Object, new Mock<IJenkinServiceFactory>().Object, new Mock<ITimerFactory>().Object);
            vm.LoginInfoRequired += this.VmOnLoginInfoRequired;
            vm.InitCurrentViewModel();

            Check.That(vm.CurrentViewModel).IsInstanceOf<LoginViewModel>();
            Check.That(this.loginInfoWasRequiredWasRequired).IsTrue();
            settings.Verify(o => o.Load(), Times.Once);
        }

        private void VmOnLoginInfoRequired(object sender, LoginInfoRequiredArgs loginInfoRequiredArgs)
        {
            this.loginInfoWasRequiredWasRequired = true;
        }

        [Test]
        public void WhenInitializedThenCurrentViewModelIsJobsIfSettingsWereLoaded()
        {
            var settings = new Mock<ISettings>();
            settings.Setup(o => o.RefreshDelay).Returns(1);
            var timeMoq = new Mock<ITimerFactory>();
            timeMoq.Setup(o => o.Create(It.IsAny<int>())).Returns(new Mock<ITimer>().Object);

            var vm = new MainViewModel(settings.Object, new Mock<IMessenger>().Object, new Mock<IUiService>().Object, new Mock<IJenkinServiceFactory>().Object, timeMoq.Object);
            vm.InitCurrentViewModel();

            Check.That(vm.CurrentViewModel).IsInstanceOf<JenkinsViewsViewModel>();
            settings.Verify(o => o.Load(), Times.Once);
        }

        [Test]
        public void WhenUserAuthenticatedThenCurrentViewModelIsViewsViewModel()
        {
            var messenger = new Messenger();
            const string ServerUrl = "serveur";
            var jobsByView = new List<JenkinsViewModel> { new JenkinsViewModel("PDA", new List<string> { "Expresso" }) };
            const string Login = "Login";
            const string ApiToken = "Token";
            const int RefreshDelay = 3;
            var settings = new Mock<ISettings>();
            settings.Setup(o => o.RefreshDelay).Returns(3);

            var vm = new MainViewModel(settings.Object, messenger, new Mock<IUiService>().Object, new Mock<IJenkinServiceFactory>().Object, new Mock<ITimerFactory>().Object);

            messenger.Send(new UserAuthenticated(ServerUrl, jobsByView, Login, ApiToken, RefreshDelay));

            Check.That(vm.CurrentViewModel).IsInstanceOf<JenkinsViewsViewModel>();
        }

        [Test]
        public void WhenBuildFailedThenTriggerEventNotificationrequired()
        {
            Check.That(this.notificationBuildWasRequired).IsFalse();
            var messenger = new Messenger();
            var vm = new MainViewModel(new Mock<ISettings>().Object, messenger, new Mock<IUiService>().Object, new Mock<IJenkinServiceFactory>().Object, new Mock<ITimerFactory>().Object);
            vm.NotificationRequired += this.VmOnNotificationRequired;

            messenger.Send(new BuildOccured(new List<JenkinsJobViewModel> { this.jenkinsJobViewModel }));

            Check.That(this.notificationBuildWasRequired).IsTrue();
        }

        [Test]
        public void WhenBuildBackToNormalThenTriggerEventNotificationrequired()
        {
            Check.That(this.notificationBuildWasRequired).IsFalse();
            var messenger = new Messenger();
            var vm = new MainViewModel(new Mock<ISettings>().Object, messenger, new Mock<IUiService>().Object, new Mock<IJenkinServiceFactory>().Object, new Mock<ITimerFactory>().Object);
            vm.NotificationRequired += this.VmOnNotificationRequired;

            messenger.Send(new BuildOccured(new List<JenkinsJobViewModel> { this.jenkinsJobViewModel }));

            Check.That(this.notificationBuildWasRequired).IsTrue();
        }

        [Test]
        public void WhenLoginViewRequiredThenCurrentVmIsLoginVm()
        {
            var messenger = new Messenger();
            var settings = new Mock<ISettings>();
            settings.Setup(o => o.JenkinsViews).Returns(new List<JenkinsViewModel> { new JenkinsViewModel("view1", new List<string> { "job" }), new JenkinsViewModel("view2") });
            var vm = new MainViewModel(
                settings.Object,
                messenger,
                new Mock<IUiService>().Object,
                new Mock<IJenkinServiceFactory>().Object,
                new Mock<ITimerFactory>().Object);

            messenger.Send(new LoginViewRequired(vm));

            Check.That(vm.CurrentViewModel).IsInstanceOf<LoginViewModel>();
            var currentVm = (LoginViewModel)vm.CurrentViewModel;

            Check.That(currentVm.ViewsAndJobs.Count).Equals(2);
            Check.That(currentVm.ViewsAndJobs[0].ViewName).Equals("view1");
            Check.That(currentVm.ViewsAndJobs[0].JobName).Equals("job");
            Check.That(currentVm.ViewsAndJobs[1].ViewName).Equals("view2");
            Check.That(currentVm.ViewsAndJobs[1].JobName).Equals("");
        }

        private void VmOnNotificationRequired(object sender, NotificationRequiredArgs notificationRequiredArgs)
        {
            this.notificationBuildWasRequired = true;
        }
    }
}
