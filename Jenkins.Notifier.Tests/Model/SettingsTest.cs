namespace Jenkins.Notifier.Tests.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Jenkins.Notifier.Exceptions;
    using Jenkins.Notifier.Model;
    using Jenkins.Notifier.Services;

    using Moq;

    using NFluent;

    using NUnit.Framework;

    [TestFixture]
    public class SettingsTest
    {
        private const string UrlServer = "Server";
        private const string Login = "Login";
        private const string Apitoken = "ApiToken";
        private const int RefreshDelay = 5;
        private readonly IList<JenkinsViewModel> jenkinsViewModels = new List<JenkinsViewModel>(); 

        [Test]
        public void WhenSaveThenFileSaveIsCallWithCurrentSettings()
        {
            var fileSettings = new Mock<IFileSettings>();
            var settings = new Settings(fileSettings.Object);
            settings.SetSettings(UrlServer, this.jenkinsViewModels, Login, Apitoken, RefreshDelay);

            Check.ThatCode(settings.Save).DoesNotThrow();
            fileSettings.Verify(o => o.Save(UrlServer, this.jenkinsViewModels, Login, Apitoken, RefreshDelay));
        }

        [Test]
        public void WhenSaveRaisedErrorThenErrorIsRaised()
        {
            var fileSettings = new Mock<IFileSettings>();
            fileSettings.Setup(
                o => o.Save(It.IsAny<string>(), It.IsAny<List<JenkinsViewModel>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception());

            var settings = new Settings(fileSettings.Object);
            settings.SetSettings(UrlServer, this.jenkinsViewModels, Login, Apitoken, RefreshDelay);

            Check.ThatCode(settings.Save).ThrowsAny();
        }

        [Test]
        public void WhenSaveBeforeSetSettingsThenErrorIsRaised()
        {
            var fileSettings = new Mock<IFileSettings>();
            var settings = new Settings(fileSettings.Object);

            Check.ThatCode(settings.Save).Throws<InvalidSettingsException>();
            fileSettings.Verify(o => o.Save(UrlServer, this.jenkinsViewModels, Login, Apitoken, RefreshDelay), Times.Never);
        }

        [Test]
        public void WhenLoadThenFileSettingsLoadIsCallAndValuesAreSet()
        {
            var fileSettings = new Mock<IFileSettings>();
            fileSettings.Setup(o => o.Load())
                .Returns(
                    new SerializableSettings
                        {
                            UrlServer = UrlServer,
                            Apitoken = Apitoken,
                            Login = Login,
                            JenkinsViews = this.jenkinsViewModels.ToList(),
                            RefreshDelay = RefreshDelay
                        });

            var settings = new Settings(fileSettings.Object);

            Check.ThatCode(settings.Load).DoesNotThrow();
            fileSettings.Verify(o => o.Load(), Times.Once);
            Check.That(settings.ServerUrl).Equals(UrlServer);
            Check.That(settings.ApiToken).Equals(Apitoken);
            Check.That(settings.Login).Equals(Login);
            Check.That(settings.JenkinsViews).IsEmpty();
            Check.That(settings.RefreshDelay).Equals(RefreshDelay);
        }

        [Test]
        public void WhenLoadRaiseErrorThenReturnFalse()
        {
            var fileSettings = new Mock<IFileSettings>();
            fileSettings.Setup(o => o.Load()).Throws(new Exception());

            var settings = new Settings(fileSettings.Object);

            Check.ThatCode(settings.Load).Throws<Exception>();
        }
    }
}
