using Jenkins.Notifier.Services;

namespace Jenkins.Notifier.Tests.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using GalaSoft.MvvmLight.Messaging;

    using Jenkins.Notifier.Events;
    using Jenkins.Notifier.Model;
    using Jenkins.Notifier.ViewModel;

    using Moq;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class LoginViewModelTests
    {
        private const string ServerUrl = "ServerUrl";
        private const string ViewToAdd = "ViewToAdd";
        private const string JobToAdd = "JobToAdd";
        private const string Login = "Login";
        private const string ApiToken = "ApiToken";
        private const int RefreshDelay = 3;
        private readonly Mock<IMessenger> messenger = new Mock<IMessenger>();
        private readonly Mock<ISettings> settings = new Mock<ISettings>();
        private readonly Mock<IUiService> uiService = new Mock<IUiService>();
        private IEnumerable<ViewAndJobViewModel> views;

        [SetUp]
        public void Init()
        {
            this.views = new List<ViewAndJobViewModel>
                        {
                            new ViewAndJobViewModel(
                                this.messenger.Object,
                                new ViewAndJobModel("PDA_050", "SIK_Mobilite-Expresso")),
                            new ViewAndJobViewModel(
                                this.messenger.Object,
                                new ViewAndJobModel("PDA_050", "SIK_Mobilite-Expresso-ls1")),
                        };
        }

        [Test]
        public void WhenValidServerAndValidUserAndValidPasswordAndSettingsAreSavedThenUserIsAuthenticatedAndThereIsNoErrorMessage()
        {
            var localSettings = new Mock<ISettings>();
            var vm = new LoginViewModel(localSettings.Object, this.messenger.Object)
                         {
                             ServerUrl = ServerUrl,
                             Login = Login,
                             ApiToken = ApiToken,
                             RefreshDelay = RefreshDelay,
                             ViewsAndJobs = new ObservableCollection<ViewAndJobViewModel>(this.views)
                         };

            Check.That(vm.LoginCommand.CanExecute(null)).IsTrue();
            vm.LoginCommand.Execute(null);

            var jenkinsViews = this.GetJenkinsViews(this.views);
            this.messenger.Verify(o => o.Send(new UserAuthenticated(ServerUrl, jenkinsViews, Login, ApiToken, RefreshDelay)), Times.Once);
            localSettings.Verify(o => o.SetSettings(ServerUrl, It.IsAny<IList<JenkinsViewModel>>(), Login, ApiToken, RefreshDelay));
            localSettings.Verify(o => o.Save());
            Check.That(vm.IsErrorDisplay).IsFalse();
        }

        [Test]
        public void WhenUserIsAuthenticatedAndWiewWithNoSpecificJobThenSaveViewWithoutJob()
        {
            var localSettings = new Mock<ISettings>();
            this.views = new List<ViewAndJobViewModel>
                        {
                            new ViewAndJobViewModel(
                                this.messenger.Object,
                                new ViewAndJobModel("PDA_050", string.Empty))
                        };

            var vm = new LoginViewModel(localSettings.Object, this.messenger.Object)
            {
                ServerUrl = ServerUrl,
                Login = Login,
                ApiToken = ApiToken,
                RefreshDelay = RefreshDelay,
                ViewsAndJobs = new ObservableCollection<ViewAndJobViewModel>(this.views)
            };

            Check.That(vm.LoginCommand.CanExecute(null)).IsTrue();
            vm.LoginCommand.Execute(null);

            localSettings.Verify(o => o.SetSettings(ServerUrl, It.Is<IList<JenkinsViewModel>>(l => l.Any(v => v.View.Equals("PDA_050") && !v.Jobs.Any())), Login, ApiToken, RefreshDelay));
        }

        private IList<JenkinsViewModel> GetJenkinsViews(IEnumerable<ViewAndJobViewModel> viewAndJobViewModels)
        {
            var jenkinsViews = new List<JenkinsViewModel>();
            foreach (var viewAndJobViewModel in viewAndJobViewModels)
            {
                if (jenkinsViews.Any(o => o.View.Equals(viewAndJobViewModel.ViewName)))
                {
                    jenkinsViews.First(o => o.View.Equals(viewAndJobViewModel.ViewName)).Jobs.Add(viewAndJobViewModel.JobName);
                }
                else
                {
                    jenkinsViews.Add(new JenkinsViewModel(viewAndJobViewModel.ViewName, new List<string> { viewAndJobViewModel.JobName }));
                }
            }

            return jenkinsViews;
        }

        [Test]
        public void WhenInvalidServerOrLoginOrTokenOrRefreshThenCannotConnect()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
            {
                ServerUrl = null,
                Login = Login,
                ApiToken = ApiToken,
                ViewsAndJobs = new ObservableCollection<ViewAndJobViewModel>(this.views)
            };

            Check.That(vm.LoginCommand.CanExecute(null)).IsFalse();
            vm.ServerUrl = ServerUrl;
            vm.Login = null;
            Check.That(vm.LoginCommand.CanExecute(null)).IsFalse();
            vm.Login = Login;
            vm.ApiToken = null;
            Check.That(vm.LoginCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void WhenNoViewToAddThenCannotAddView()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object);

            Check.That(vm.AddViewAndJobCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void WhenOnlyJobToAddThenCannotExecute()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
            {
                JobToAdd = JobToAdd,
            };

            Check.That(vm.AddViewAndJobCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void WhenOnlyViewToAddThenCannotExecute()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
            {
                ViewToAdd = ViewToAdd,
            };

            Check.That(vm.AddViewAndJobCommand.CanExecute(null)).IsTrue();
        }

        [Test]
        public void WhenViewAndJobToAddThenViewIsAdded()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
                         {
                             ViewToAdd = ViewToAdd,
                             JobToAdd = JobToAdd
                         };

            Check.That(vm.AddViewAndJobCommand.CanExecute(null)).IsTrue();
            vm.AddViewAndJobCommand.Execute(null);

            Check.That(vm.ViewsAndJobs.First().ViewName).Equals(ViewToAdd);
            Check.That(vm.ViewsAndJobs.First().JobName).Equals(JobToAdd);
        }

        [Test]
        public void WhenViewToAddThenViewIsAdded()
        {
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
            {
                ViewToAdd = ViewToAdd
            };

            Check.That(vm.AddViewAndJobCommand.CanExecute(null)).IsTrue();
            vm.AddViewAndJobCommand.Execute(null);

            Check.That(vm.ViewsAndJobs.First().ViewName).Equals(ViewToAdd);
            Check.That(vm.ViewsAndJobs.First().JobName).IsEmpty();
        }

        [Test]
        public void WhenRemoveViewThenViewIsRemoved()
        {
            var messengerLocal = new Messenger();
            var viewsLocal = new List<ViewAndJobViewModel>
                                 {
                                     new ViewAndJobViewModel(
                                         messengerLocal,
                                         new ViewAndJobModel(this.views.First().ViewName, this.views.First().JobName))
                                 };
            var vm = new LoginViewModel(this.settings.Object, messengerLocal)
                         {
                             ViewsAndJobs =
                                 new ObservableCollection<ViewAndJobViewModel>(viewsLocal)
                         };

            messengerLocal.Send(new RemoveViewAndJobRequired(viewsLocal.First()));
            Check.That(vm.ViewsAndJobs.Any()).IsFalse();
        }

        [Test]
        public void WhenUserAuthenticatedThenErrorIsDisplayIfSettingsAreNotSaved()
        {
            this.settings.Setup(o => o.Save()).Throws(new Exception());
            var vm = new LoginViewModel(this.settings.Object, this.messenger.Object)
            {
                ServerUrl = ServerUrl,
                Login = Login,
                ApiToken = ApiToken,
                RefreshDelay = RefreshDelay,
                ViewsAndJobs = new ObservableCollection<ViewAndJobViewModel>(this.views)
            };

            vm.LoginCommand.Execute(null);

            Check.That(vm.IsErrorDisplay).IsTrue();
            Check.That(vm.ErrorViewModel).IsNotNull();
        }
    }
}
