using System;

using Jenkins.Core;
using Jenkins.Notifier.ViewModel.Commands;

using Moq;

using NFluent;

using NUnit.Framework;

namespace Jenkins.Notifier.Tests.ViewModel
{
    [TestFixture]
    public class RunBuildCommandTest
    {
        private Mock<IJenkinsRestClient> jenkinsRestClientMock;

        [SetUp]
        public void Init()
        {
            this.jenkinsRestClientMock = new Mock<IJenkinsRestClient>();
        }

        [Test]
        public void WhenExecuteThenJenkinsRestClientRunJobIsCalled()
        {
            const string BuildUrl = "Build Url";
            var runBuildCommand = new RunBuildCommand(this.jenkinsRestClientMock.Object, BuildUrl);

            runBuildCommand.ExecuteAsync().Wait();

            this.jenkinsRestClientMock.Verify(job => job.RunJobAsync(BuildUrl), Times.Once);
        }

        [Test]
        public void WhenExecuteAndThrowExceptionShouldCatchIt()
        {
            const string BuildUrl = "Build Url";
            this.jenkinsRestClientMock.Setup(x => x.RunJobAsync(BuildUrl)).Throws<Exception>();
            var stopBuildCommand = new RunBuildCommand(this.jenkinsRestClientMock.Object, BuildUrl);

            Check.ThatAsyncCode(async () => await stopBuildCommand.ExecuteAsync()).DoesNotThrow();
        }

        [Test]
        public void WhenBuildUrlIsNullThenCommandIsNotExecutable()
        {
            var runBuildCommand = new RunBuildCommand(this.jenkinsRestClientMock.Object, null);

            Check.That(runBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void WhenBuildUrlIsEmptyThenCommandIsNotExecutable()
        {
            var runBuildCommand = new RunBuildCommand(this.jenkinsRestClientMock.Object, string.Empty);

            Check.That(runBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public async void WhenExecuteThenCommandIsNotExecutable()
        {
             const string BuildUrl = "Build Url";
            var runBuildCommand = new RunBuildCommand(this.jenkinsRestClientMock.Object, BuildUrl);

            Check.That(runBuildCommand.CanExecute(null)).IsTrue();

            await runBuildCommand.ExecuteAsync();

            Check.That(runBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void RunBuildCommandEqualOtherRunBuildCommandWhenShareSameBuildUrl()
        {
            const string BuildUrl = "Build Url";
            var runBuildCommand1 = new RunBuildCommand(this.jenkinsRestClientMock.Object, BuildUrl);
            var runBuildCommand2 = new RunBuildCommand(this.jenkinsRestClientMock.Object, BuildUrl);
            var runBuildCommand3 = new RunBuildCommand(this.jenkinsRestClientMock.Object, "fake url");

            Check.That(runBuildCommand1).Equals(runBuildCommand2);
            Check.That(runBuildCommand1).IsNotEqualTo(runBuildCommand3);
        }
    }
}
