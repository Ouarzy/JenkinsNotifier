using Jenkins.Notifier.ViewModel.Commands;

namespace Jenkins.Notifier.Tests.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight.Messaging;
    using Core;
    using Events;
    using Notifier.Model;
    using Notifier.Services;
    using Notifier.ViewModel;
    using Moq;
    using NFluent;
    using NUnit.Framework;

    [TestFixture]
    public class JenkinsViewsViewModelTest
    {
        private const string UrlJob1 = "Url1";

        private const string UrlJob2 = "Url2";

        private readonly RunBuildCommand buildCommandJob1 = new RunBuildCommand(new Mock<IJenkinsRestClient>().Object, "BuildUrl1");

        private readonly RunBuildCommand buildCommandJob2 = new RunBuildCommand(new Mock<IJenkinsRestClient>().Object, "BuildUrl2");

        private readonly StopBuildCommand stopBuildCommandJob1 = new StopBuildCommand(new Mock<IJenkinsRestClient>().Object, "StopBuildUrl1");

        private readonly StopBuildCommand stopBuildCommandJob2 = new StopBuildCommand(new Mock<IJenkinsRestClient>().Object, "StopBuildUrl2");

        private readonly IEnumerable<JenkinsViewViewModel> listViews;

        private readonly JenkinsJobViewModel job1;

        private readonly JenkinsJobViewModel job2;

        private readonly List<JenkinsJobViewModel> jobs;

        public JenkinsViewsViewModelTest()
        {
            this.job1 = new JenkinsJobViewModel("job1", JobStatus.Success, UrlJob1, 1265, this.buildCommandJob1, this.stopBuildCommandJob1);
            this.job2 = new JenkinsJobViewModel("job2", JobStatus.Failed, UrlJob2, 5445, this.buildCommandJob2, this.stopBuildCommandJob2);

            this.jobs = new List<JenkinsJobViewModel> {
                this.job1,
                this.job2 };

            this.listViews = new List<JenkinsViewViewModel> 
                        {
                            new JenkinsViewViewModel("view", this.jobs)
                        };
        }

        [Test]
        public async Task WhenJenkinsViewsViewModelIsRefreshThenViewsAreListedAsync()
        {
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => this.listViews);
                    task.Start();
                    return task;
                });
            var vm = new JenkinsViewsViewModel(
                new Mock<IMessenger>().Object,
                jenkinsServiceMoq.Object,
                new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);

            Check.That(vm.SelectedIndex).Equals(0);
            Check.That(vm.JenkinsViews).ContainsExactly(this.listViews);
        }

        [Test]
        public void WhenTimerTickThenViewsAreListedAsync()
        {
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            var timerFake = new TimerFake();

            // ReSharper disable once ObjectCreationAsStatement
            new JenkinsViewsViewModel(new Mock<IMessenger>().Object, jenkinsServiceMoq.Object, timerFake);

            timerFake.RaiseTick();

            jenkinsServiceMoq.Verify(o => o.Views(), Times.Once);
        }

        [Test]
        public void WhenPreviousViewCommandThenLoginViewRequired()
        {
            var messenger = new Mock<IMessenger>();
            var vm = new JenkinsViewsViewModel(
                messenger.Object,
                new Mock<IJenkinService>().Object,
                new Mock<ITimer>().Object);

            vm.Previous.Execute(null);

            messenger.Verify(o => o.Send(new LoginViewRequired(vm)));
        }

        [Test]
        public async Task WhenInitialJobsHaveNoFailedThenNoEventIsSent()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsViews = new List<JenkinsViewViewModel>
                                   {
                                       new JenkinsViewViewModel("view", new List<JenkinsJobViewModel>
                                        {
                                            this.job1, new JenkinsJobViewModel("job2",JobStatus.Success,UrlJob2,564, this.buildCommandJob2, this.stopBuildCommandJob2)
                                        })
                                   };

            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => jenkinsViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);

            messenger.Verify(o => o.Send(new BuildOccured(null)), Times.Never());
        }

        [Test]
        public async Task WhenInitialJobsHaveFailedThenBuildFailedIsSent()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => this.listViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);

            messenger.Verify(
                o =>
                o.Send(
                    new BuildOccured(
                    new List<JenkinsJobViewModel>
                        {
                            new JenkinsJobViewModel("job2", JobStatus.Failed, UrlJob2, 1234, It.IsAny<RunBuildCommand>(), It.IsAny<StopBuildCommand>(), false)
                        })),
                Times.Once);
        }

        [Test]
        public async Task WhenNothingChangeSincePreviousRefreshThenThereIsNoNewNotification()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => this.listViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Once);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Once);
        }

        [Test]
        public async Task WhenBreakBuildBackToNormalThenBuildBackToNormalIsSendOnce()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsViews = new List<JenkinsViewViewModel>
                                   {
                                       new JenkinsViewViewModel("view", new List<JenkinsJobViewModel>
                                               {
                                                   new JenkinsJobViewModel("job1", JobStatus.Failed, UrlJob1, 1265, this.buildCommandJob1, this.stopBuildCommandJob1),
                                                   new JenkinsJobViewModel("job2", JobStatus.Failed, UrlJob2, 5454, this.buildCommandJob2, this.stopBuildCommandJob2)
                                               })
                                   };
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    // ReSharper disable once AccessToModifiedClosure
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => jenkinsViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Once);

            jenkinsViews = new List<JenkinsViewViewModel>
                               {
                                   new JenkinsViewViewModel("view",new List<JenkinsJobViewModel>
                                           {
                                               new JenkinsJobViewModel("job1",JobStatus.Failed,UrlJob1, 5445, this.buildCommandJob1, this.stopBuildCommandJob1),
                                               new JenkinsJobViewModel("job2",JobStatus.Success,UrlJob2, 5454, this.buildCommandJob2, this.stopBuildCommandJob2)
                                           })
                               };

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(
                o =>
                o.Send(
                    new BuildOccured(
                    new List<JenkinsJobViewModel>
                        {
                            new JenkinsJobViewModel("job2", JobStatus.Success, UrlJob2, 5454, this.buildCommandJob2, this.stopBuildCommandJob2, false)
                        })),
                Times.Once);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Exactly(2));

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Exactly(2));
        }

        [Test]
        public async Task WhenChangeSincePreviousRefreshThenThereIsNewNotification()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsViews = new List<JenkinsViewViewModel>
                                   {
                                       new JenkinsViewViewModel("view",new List<JenkinsJobViewModel>
                                               {
                                                   this.job1,new JenkinsJobViewModel("job2",JobStatus.Success,UrlJob2, 8764, this.buildCommandJob2, this.stopBuildCommandJob2)
                                               })
                                   };
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    // ReSharper disable once AccessToModifiedClosure
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => jenkinsViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(new BuildOccured(null)), Times.Never);

            jenkinsViews = new List<JenkinsViewViewModel>
                               {
                                   new JenkinsViewViewModel("view",new List<JenkinsJobViewModel>
                                           {
                                               new JenkinsJobViewModel("job1",JobStatus.Failed,UrlJob1, 5454, this.buildCommandJob1, this.stopBuildCommandJob1),
                                               new JenkinsJobViewModel("job2",JobStatus.Success,UrlJob1, 1231, this.buildCommandJob2, this.stopBuildCommandJob2)
                                           })
                               };

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(
                o =>
                o.Send(
                    new BuildOccured(
                    new List<JenkinsJobViewModel>
                        {
                            new JenkinsJobViewModel("job1", JobStatus.Failed, UrlJob1, 5454, this.buildCommandJob1, this.stopBuildCommandJob1, false)
                        })),
                Times.Once);
        }

        [Test]
        public async Task WhenRequestFailedThenErrorMessageIsDisplayedAndIsLoadingIsFalse()
        {
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Throws(new Exception());
            var vm = new JenkinsViewsViewModel(
                new Mock<IMessenger>().Object,
                jenkinsServiceMoq.Object,
                new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);

            Check.That(vm.IsErrorDisplay).IsTrue();
            Check.That(vm.IsLoading).IsFalse();
        }

        [Test]
        public async Task WhenLastJobsHaveFailedAndNewJobsFailedThenThereIsNewNotification()
        {
            var messenger = new Mock<IMessenger>();
            var jenkinsServiceMoq = new Mock<IJenkinService>();
            jenkinsServiceMoq.Setup(o => o.Views()).Returns(
                () =>
                {
                    var task = new Task<IEnumerable<JenkinsViewViewModel>>(() => this.listViews);
                    task.Start();
                    return task;
                });

            var vm = new JenkinsViewsViewModel(messenger.Object, jenkinsServiceMoq.Object, new Mock<ITimer>().Object);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Once);

            this.jobs[1] = new JenkinsJobViewModel(this.job2.DisplayName, JobStatus.Failed, UrlJob2, this.job2.LastRanBuildNumber + 1, this.buildCommandJob2, this.stopBuildCommandJob2);

            await vm.RefreshJobs.ExecuteAsync(null);
            messenger.Verify(o => o.Send(It.IsAny<BuildOccured>()), Times.Exactly(2));
        }
    }
}