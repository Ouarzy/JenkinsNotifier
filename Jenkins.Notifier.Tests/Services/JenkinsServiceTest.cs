using Jenkins.Notifier.ViewModel.Commands;

namespace Jenkins.Notifier.Tests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Core;
    using Domain;
    using Notifier.Model;
    using Notifier.Services;

    using Moq;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class JenkinsServiceTest
    {
        private const string Url = "Url";
        private const string Job1 = "Expresso";
        private const string Job2 = "BluCofi";
        private const string Job3 = "Confluence";
        private const string Job4 = "Deletion";
        private const string View1 = "PDA_050";
        private const string View2 = "092_Tools";
        private const int BuildNumber1 = 56;
        private const int BuildNumber2 = 57;
        private const int BuildNumber3 = 58;
        private const int BuildNumber4 = 59;
        private const int BuildNumberfake = 999;
        private const string Url1 = "url1";
        private const string Url2 = "url2";
        private const string Url3 = "url3";
        private const string Url4 = "url4";

        [Test]
        public void WhenJenkinsServiceIsCreatedWithNullOrEmptyJobThenAllJobsAreListed()
        {
            var restMock = new Mock<IJenkinsRestClient>();

            var jenkinsService = new JenkinsService(restMock.Object, Url, null);
            var views = jenkinsService.Views();

            restMock.Verify(o => o.GetServerAsync(Url, false));
            Check.That(views).IsNotNull();
        }

        [Test]
        public async Task WhenViewWithoutSpecificJobsIsRequiredThenViewIsCalledWithAllJobs()
        {

            // Arrange
            var restMock = new Mock<IJenkinsRestClient>();
            restMock.Setup(o => o.GetViewAsync(this.GetApiString(View1), false)).Returns(
                () =>
                {
                    var task =
                        new Task<JenkinsView>(
                            () =>
                            new JenkinsView
                            {
                                Name = View1,
                                Url = Url,
                                Jobs =
                                    new List<JenkinsJob>
                                                {
                                                    new JenkinsJob { DisplayName = Job1, Status = new JenkinsJobStatus(), Url = Url1 },
                                                    new JenkinsJob { DisplayName = Job2, Status = new JenkinsJobStatus(), Url = Url2 }
                                                }
                                    .ToArray()
                            });
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url1)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url1,
                                LastCompletedBuild = new JenkinsBuild { Number = BuildNumber1 },
                                LastFailedBuild = new JenkinsBuild { Number = BuildNumberfake },
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber1, Status = JenkinsBuildStatus.Success } }
                            }
                        );
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url2)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url2,
                                LastCompletedBuild = new JenkinsBuild { Number = BuildNumberfake },
                                LastFailedBuild = new JenkinsBuild { Number = BuildNumber2 },
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber2, Status = JenkinsBuildStatus.Unstable } }
                            }
                        );
                    task.Start();
                    return task;
                });

            // Act
            var jenkinsService = new JenkinsService(
                restMock.Object,
                Url,
                new List<JenkinsViewModel>
                    {
                        new JenkinsViewModel(View1)
                    });
            var views = await jenkinsService.Views();

            // Asserts
            restMock.Verify(o => o.GetViewAsync(this.GetApiString(View1), false), Times.Once);

            var jenkinsViewViewModels = views as IList<JenkinsViewViewModel> ?? views.ToList();
            Check.That(jenkinsViewViewModels[0].ViewName).Equals(View1);

            var jobsView0 = jenkinsViewViewModels[0].Jobs.ToList();
            Check.That(jobsView0[0].DisplayName).Equals(Job1);
            Check.That(jobsView0[0].LastRanBuildNumber).Equals(BuildNumber1);
            Check.That(jobsView0[0].BuildRunning).IsFalse();
            Check.That(jobsView0[0].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url1 + "build"));
            Check.That(jobsView0[1].DisplayName).Equals(Job2);
            Check.That(jobsView0[1].LastRanBuildNumber).Equals(BuildNumber2);
            Check.That(jobsView0[1].BuildRunning).IsFalse();
            Check.That(jobsView0[1].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url2 + "build"));
        }

        [Test]
        public async Task WhenViewsAreRequiredThenViewsAreCalled()
        {
            // Arrange
            var restMock = new Mock<IJenkinsRestClient>();
            restMock.Setup(o => o.GetViewAsync(this.GetApiString(View1), false)).Returns(
                () =>
                {
                    var task =
                        new Task<JenkinsView>(
                            () =>
                            new JenkinsView
                                {
                                    Name = View1,
                                    Url = Url,
                                    Jobs =
                                        new List<JenkinsJob>
                                                {
                                                    new JenkinsJob { DisplayName = Job1, Status = new JenkinsJobStatus(), Url = Url1 },
                                                    new JenkinsJob { DisplayName = Job2, Status = new JenkinsJobStatus(), Url = Url2 }
                                                }
                                        .ToArray()
                                });
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetViewAsync(this.GetApiString(View2), false)).Returns(
                () =>
                {
                    var task =
                        new Task<JenkinsView>(
                            () =>
                                new JenkinsView
                                {
                                    Name = View2,
                                    Url = Url,
                                    Jobs =
                                        new List<JenkinsJob>
                                                {
                                                    new JenkinsJob { DisplayName = Job3, Status = new JenkinsJobStatus(), Url = Url3 },
                                                    new JenkinsJob { DisplayName = Job4, Status = new JenkinsJobStatus(), Url = Url4 }
                                                }
                                        .ToArray()
                                });
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url1)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url1,
                                LastCompletedBuild = new JenkinsBuild { Number = BuildNumber1 },
                                LastFailedBuild = new JenkinsBuild { Number = BuildNumberfake },
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber1, Status = JenkinsBuildStatus.Success } }
                            }
                        );
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url2)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url2,
                                LastCompletedBuild = new JenkinsBuild { Number = BuildNumberfake },
                                LastFailedBuild = new JenkinsBuild { Number = BuildNumber2 },
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber2, Status = JenkinsBuildStatus.Unstable} }
                            }
                        );
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url3)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url3,
                                LastCompletedBuild = new JenkinsBuild { Number = BuildNumber3 },
                                LastFailedBuild = null,
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber3, Status = JenkinsBuildStatus.Success } }
                            }
                        );
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url4)))).Returns(
                () =>
                {
                    var task = new Task<JenkinsJob>(
                        () =>
                            new JenkinsJob
                            {
                                Url = Url4,
                                LastCompletedBuild = null,
                                LastFailedBuild = new JenkinsBuild { Number = BuildNumber4 },
                                Builds = new[] { new JenkinsBuild { Number = BuildNumberfake + 1 }, new JenkinsBuild { Number = BuildNumber4 } }
                            }
                        );
                    task.Start();
                    return task;
                });

            // Act
            var jenkinsService = new JenkinsService(
                restMock.Object,
                Url,
                new List<JenkinsViewModel>
                    {
                        new JenkinsViewModel(View1, new List<string> { Job1, Job2 }),
                        new JenkinsViewModel(View2, new List<string> { Job3, Job4 })
                    });
            var views = await jenkinsService.Views();

            // Asserts
            restMock.Verify(o => o.GetViewAsync(this.GetApiString(View1), false), Times.Once);
            restMock.Verify(o => o.GetViewAsync(this.GetApiString(View2), false), Times.Once);

            var jenkinsViewViewModels = views as IList<JenkinsViewViewModel> ?? views.ToList();
            Check.That(jenkinsViewViewModels[0].ViewName).Equals(View1);
            Check.That(jenkinsViewViewModels[1].ViewName).Equals(View2);

            var jobsView0 = jenkinsViewViewModels[0].Jobs.ToList();
            Check.That(jobsView0[0].DisplayName).Equals(Job1);
            Check.That(jobsView0[0].LastRanBuildNumber).Equals(BuildNumber1);
            Check.That(jobsView0[0].BuildRunning).IsFalse();
            Check.That(jobsView0[0].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url1 + "build"));
            Check.That(jobsView0[1].DisplayName).Equals(Job2);
            Check.That(jobsView0[1].LastRanBuildNumber).Equals(BuildNumber2);
            Check.That(jobsView0[1].BuildRunning).IsFalse();
            Check.That(jobsView0[1].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url2 + "build"));

            var jobsView1 = jenkinsViewViewModels[1].Jobs.ToList();
            Check.That(jobsView1[0].DisplayName).Equals(Job3);
            Check.That(jobsView1[0].LastRanBuildNumber).Equals(BuildNumber3);
            Check.That(jobsView1[0].BuildRunning).IsFalse();
            Check.That(jobsView1[0].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url3 + "build"));
            Check.That(jobsView1[1].DisplayName).Equals(Job4);
            Check.That(jobsView1[1].LastRanBuildNumber).Equals(BuildNumber4);
            Check.That(jobsView1[1].BuildRunning).IsTrue();
            Check.That(jobsView1[1].RunBuildOnline).Equals(new RunBuildCommand(It.IsAny<IJenkinsRestClient>(), Url4 + "build"));
        }

        [Test]
        [TestCase(true, Url3 + "stop")]
        [TestCase(false, "")]
        public async Task WhenViewIsRequieredThenProvideRunningJobUrlToStopCommand(bool jobRunning, string expectedStopRestUrl)
        {
            // Arrange
            var restMock = new Mock<IJenkinsRestClient>();
            restMock.Setup(o => o.GetViewAsync(this.GetApiString(View1), false)).Returns(() =>
                {
                    var task = new Task<JenkinsView>(() =>
                            new JenkinsView
                            {
                                Name = View1,
                                Url = Url,
                                Jobs = new[]
                                {
                                    new JenkinsJob { DisplayName = Job1, Status = new JenkinsJobStatus(), Url = Url1 },
                                }
                            });
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url1)))).Returns(() =>
                {
                    var task = new Task<JenkinsJob>(() => new JenkinsJob
                            {
                                Url = Url1,
                                LastCompletedBuild = new JenkinsBuild { Number = jobRunning ? BuildNumber1 : BuildNumberfake },
                                LastFailedBuild = null,
                                Builds = new[] { new JenkinsBuild { Number = BuildNumber1, Url = Url2 }, new JenkinsBuild { Number = BuildNumber2, Url = Url3 }, new JenkinsBuild { Number = BuildNumber3, Url = Url4 } }
                            }
                        );
                    task.Start();
                    return task;
                });

            // Act
            var jenkinsService = new JenkinsService(
                restMock.Object,
                Url,
                new List<JenkinsViewModel>
                    {
                        new JenkinsViewModel(View1, new List<string> { Job1 }),
                    });
            var views = await jenkinsService.Views();

            // Asserts
            var jobsView = views.Single().Jobs.ToList();
            Check.That(jobsView[0].StopBuildOnline).Equals(new StopBuildCommand(It.IsAny<IJenkinsRestClient>(), expectedStopRestUrl));
        }
        
        [Test]
        [TestCase(true, Url3 + "stop")]
        [TestCase(false, "")]
        public async Task WhenEmptyViewIsRequieredThenProvideRunningJobUrlToStopCommand(bool jobRunning, string expectedStopRestUrl)
        {
            // Arrange
            var restMock = new Mock<IJenkinsRestClient>();
            restMock.Setup(x => x.GetServerAsync(Url, false)).Returns(() =>
                {
                    var task = new Task<JenkinsServer>(() => new JenkinsServer { Node = new JenkinsNode{ Jobs = new[]
                                {
                                    new JenkinsJob { DisplayName = Job1, Status = new JenkinsJobStatus(), Url = Url1 },
                                } }}
                        );
                    task.Start();
                    return task;
                });

            restMock.Setup(o => o.GetJobAsync(It.Is<string>(s => s.Contains(Url1)))).Returns(() =>
            {
                var task = new Task<JenkinsJob>(() => new JenkinsJob
                {
                    Url = Url1,
                    LastCompletedBuild = new JenkinsBuild { Number = jobRunning ? BuildNumber1 : BuildNumberfake },
                    LastFailedBuild = null,
                    Builds = new[] { new JenkinsBuild { Number = BuildNumber1, Url = Url2 }, new JenkinsBuild { Number = BuildNumber2, Url = Url3 }, new JenkinsBuild { Number = BuildNumber3, Url = Url4 } }
                });
                task.Start();
                return task;
            });

            // Act
            var jenkinsService = new JenkinsService(restMock.Object, Url, null);
            var views = await jenkinsService.Views();

            // Asserts
            var jobsView = views.Single().Jobs.ToList();
            Check.That(jobsView[0].StopBuildOnline).Equals(new StopBuildCommand(It.IsAny<IJenkinsRestClient>(), expectedStopRestUrl));
        }

        private string GetApiString(string viewName)
        {
            return Url + "/view/" + viewName + "/api/json";
        }
    }
}
