using System;

using Jenkins.Core;
using Jenkins.Notifier.ViewModel.Commands;
using Moq;

using NFluent;

using NUnit.Framework;

namespace Jenkins.Notifier.Tests.ViewModel
{
    [TestFixture]
    public class StopBuildCommandTest
    {
        private Mock<IJenkinsRestClient> jenkinsRestClientMock;

        [SetUp]
        public void Init()
        {
            this.jenkinsRestClientMock = new Mock<IJenkinsRestClient>();
        }

        [Test]
        public async void WhenExecuteThenJenkinsRestClientStopJobIsCalled()
        {
            const string StopBuildUrl = "stop url";
            var stopBuildCommand = new StopBuildCommand(this.jenkinsRestClientMock.Object, StopBuildUrl);

            await stopBuildCommand.ExecuteAsync();

            this.jenkinsRestClientMock.Verify(job => job.StopJobAsync(StopBuildUrl), Times.Once);
        }

        [Test]
        public void WhenExecuteAndThrowExceptionShouldCatchIt()
        {
            const string StopBuildUrl = "stop url";
            this.jenkinsRestClientMock.Setup(x => x.StopJobAsync(StopBuildUrl)).Throws<Exception>();
            var stopBuildCommand = new StopBuildCommand(this.jenkinsRestClientMock.Object, StopBuildUrl);

            Check.ThatAsyncCode(async () => await stopBuildCommand.ExecuteAsync()).DoesNotThrow();
        }

        [Test]
        public void WhenStopBuildUrlIsNullThenCommandIsNotExecutable()
        {
            var stopBuildCommand = new StopBuildCommand(this.jenkinsRestClientMock.Object, null);

            Check.That(stopBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void WhenStopBuildUrlIsEmptyThenCommandIsNotExecutable()
        {
            var stopBuildCommand = new StopBuildCommand(this.jenkinsRestClientMock.Object, string.Empty);

            Check.That(stopBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public async void WhenExecuteThenCommandIsNotExecutable()
        {
            const string StopBuildUrl = "stop url";
            var stopBuildCommand = new StopBuildCommand(this.jenkinsRestClientMock.Object, StopBuildUrl);

            Check.That(stopBuildCommand.CanExecute(null)).IsTrue();

            await stopBuildCommand.ExecuteAsync();

            Check.That(stopBuildCommand.CanExecute(null)).IsFalse();
        }

        [Test]
        public void StopBuildCommandEqualsOtherStopBuildCommandWhenShareSameBuildUrl()
        {
            const string StopBuildUrl = "stop url";
            var stopBuildCommand1 = new StopBuildCommand(this.jenkinsRestClientMock.Object, StopBuildUrl);
            var stopBuildCommand2 = new StopBuildCommand(this.jenkinsRestClientMock.Object, StopBuildUrl);
            var stopBuildCommand3 = new StopBuildCommand(this.jenkinsRestClientMock.Object, "fake url");

            Check.That(stopBuildCommand1).Equals(stopBuildCommand2);
            Check.That(stopBuildCommand1).IsNotEqualTo(stopBuildCommand3);
        }
    }
}
