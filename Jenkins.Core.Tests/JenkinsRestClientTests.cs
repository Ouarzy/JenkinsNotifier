// --------------------------------------------------------------------------------------------------------------------
// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// <summary>
//   Defines the JenkinsRestClientTests type.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace Jenkins.Core.Tests
{
    using System;
    using System.Threading.Tasks;
    using Domain;

    using Moq;

    using NUnit.Framework;

    [TestFixture]
    public class JenkinsRestClientTests
    {
        private JenkinsRestClient defaultClient;
        private Mock<IRestManager> restManager;
        private const string Url = "url";

        [SetUp]
        public void InitContext()
        {
            this.restManager = new Mock<IRestManager>();
            this.defaultClient = new JenkinsRestClient(this.restManager.Object);
        }

        [Test]
        public void InvokeConstructorWithNullRestManagerShouldFailed()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new JenkinsRestClient(null));
        }

        [Test]
        public void InvokeGetServerAsyncWithNullUrlShouldFailed()
        {
            try
            {
                // ReSharper disable once CSharpWarnings::CS4014
                this.defaultClient.GetServerAsync(null);
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeGetJobAsyncWithNullUrlShouldFailed()
        {
            try
            {
                this.defaultClient.GetJobAsync(null).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeGetViewAsyncWithNullUrlShouldFailed()
        {
            try
            {
                this.defaultClient.GetViewAsync(null).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeGetBuildAsyncWithNullUrlShouldFailed()
        {
            try
            {
                this.defaultClient.GetBuildAsync(null).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeRunJobAsyncWithNullJobShouldFailed()
        {
            try
            {
                this.defaultClient.RunJobAsync(null).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeStopJobAsyncWithEmptyUrlShouldFailed()
        {
            try
            {
                this.defaultClient.StopJobAsync(string.Empty).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeStopJobAsyncWithNullUrlShouldFailed()
        {
            try
            {
                this.defaultClient.StopJobAsync(null).Wait();
            }
            catch (AggregateException aggEx)
            {
                Assert.IsTrue(aggEx.InnerException is ArgumentNullException);
            }
        }

        [Test]
        public void InvokeGetServerAsyncWithNoDepthLoadShouldCallLoadAsync()
        {
            this.defaultClient.GetServerAsync(Url).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsNode>(It.IsAny<string>()), Times.Once);
            this.restManager.Verify(m => m.LoadAsync<JenkinsServer>(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void InvokeGetJobAsyncWithNoDepthLoadShouldCallLoadAsync()
        {
            this.defaultClient.GetJobAsync(Url, false).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsJob>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Test]
        public void InvokeGetJobAsyncWithDepthLoadShouldCallLoadAsync()
        {
            var builds = Init3JenkinsBuilds();
            var job = InitJenkinsJob(builds);

            restManager.Setup(m => m.LoadAsync<JenkinsJob>(It.IsAny<string>()))
                       .Returns((string jobRestUrl) =>
                       {
                           var tcs = new TaskCompletionSource<JenkinsJob>();
                           tcs.SetResult(job);
                           return tcs.Task;
                       });

            restManager.Setup(m => m.LoadAsync<JenkinsBuild>(It.IsAny<string>()))
                       .Returns((string buildRestUrl) =>
                       {
                           var tcs = new TaskCompletionSource<JenkinsBuild>();
                           tcs.SetResult(builds.First(b => string.Equals(b.RestUrl, buildRestUrl, StringComparison.OrdinalIgnoreCase)));
                           return tcs.Task;
                       });

            this.defaultClient.GetJobAsync(Url, true).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsJob>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
            foreach (var build in builds)
            {
                restManager.Verify(m => m.LoadAsync<JenkinsBuild>(It.Is<string>(u => string.Equals(u, build.RestUrl, StringComparison.OrdinalIgnoreCase))), Times.Once);
            }
        }

        [Test]
        public void InvokeGetJobAsyncWithoutDepthInfosShouldCallLoadAsync()
        {
            var builds = Init3JenkinsBuilds();
            var job = InitJenkinsJob(builds);

            restManager.Setup(m => m.LoadAsync<JenkinsJob>(It.IsAny<string>()))
                       .Returns((string jobRestUrl) =>
                       {
                           var tcs = new TaskCompletionSource<JenkinsJob>();
                           tcs.SetResult(job);
                           return tcs.Task;
                       });
            this.defaultClient.GetJobAsyncWithoutDepthInfos(Url).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsJob>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Test]
        public void InvokeGetBuildAsyncWithNoDepthLoadShouldCallLoadAsync()
        {
            this.defaultClient.GetBuildAsync(Url, false).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsBuild>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Test]
        public void InvokeGetBuildAsyncWithDepthLoadShouldCallLoadAsync()
        {
            restManager.Setup(m => m.LoadAsync<JenkinsBuild>(It.IsAny<string>())).Returns(() =>
            {
                var tcs = new TaskCompletionSource<JenkinsBuild>();
                tcs.SetResult(new JenkinsBuild
                              {
                                  Url = Url
                              });
                return tcs.Task;
            });

            this.defaultClient.GetBuildAsync(Url, true).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsBuild>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
            this.restManager.Verify(m => m.LoadAsync<JenkinsTestReport>(It.Is<string>(u => string.Equals(u, Url + "testReport/api/json", StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Test]
        public void InvokeGetViewAsyncWithNoDepthLoadShouldCallLoadAsync()
        {
            this.defaultClient.GetViewAsync(Url, false).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsView>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
        }

        [Test]
        public void InvokeGetViewAsyncWithDepthLoadShouldCallLoadAsync()
        {
            var jobs = new[] { new JenkinsJob { Url = "monUrlJob" } };
            var view = new JenkinsView 
            {
                Url = Url,
                Jobs = jobs
            };

            restManager.Setup(m => m.LoadAsync<JenkinsView>(It.IsAny<string>()))
                       .Returns(() =>
                       {
                           var tcs = new TaskCompletionSource<JenkinsView>();
                           tcs.SetResult(view);
                           return tcs.Task;
                       });

            restManager.Setup(m => m.LoadAsync<JenkinsJob>(It.IsAny<string>()))
                       .Returns((string jobRestUrl) =>
                       {
                           var tcs = new TaskCompletionSource<JenkinsJob>();
                           tcs.SetResult(jobs.First(j=>string.Equals(j.RestUrl, jobRestUrl, StringComparison.OrdinalIgnoreCase)));
                           return tcs.Task;
                       });

            restManager.Setup(m => m.LoadAsync<JenkinsBuild>(It.IsAny<string>()))
                      .Returns((string buildRestUrl) =>
                      {
                          var tcs = new TaskCompletionSource<JenkinsBuild>();
                          tcs.SetResult(new JenkinsBuild());
                          return tcs.Task;
                      });

            this.defaultClient.GetViewAsync(Url, true).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsView>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);
            foreach (var job in view.Jobs)
            {
                restManager.Verify(m => m.LoadAsync<JenkinsJob>(It.Is<string>(u => string.Equals(u, job.RestUrl, StringComparison.OrdinalIgnoreCase))), Times.Once);
            }
        }

        [Test]
        public void InvokeGetTestReportAsyncShouldCallLoadAsync()
        {
            this.defaultClient.GetTestReportAsync(Url).Wait();

            this.restManager.Verify(m => m.LoadAsync<JenkinsTestReport>(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))));
        }

        [Test]
        public void InvokeRunJobAsyncShouldCallPostDataAsync()
        {
            var job = new JenkinsJob { Url = "MonJobUrl" };
            this.defaultClient.RunJobAsync(job.BuildRestUrl).Wait();

            this.restManager.Verify(m => m.PostDataAsync(It.Is<string>(u => string.Equals(u, job.BuildRestUrl, StringComparison.OrdinalIgnoreCase)), It.Is<string>(d => string.Equals(d, string.Empty))), Times.Once);
        }

        [Test]
        public void InvokeStopJobAsyncShouldCallPostDataAsync()
        {
            const string StopRestUrl = "stop url";
            this.defaultClient.StopJobAsync(StopRestUrl).Wait();

            this.restManager.Verify(m => m.PostDataAsync(StopRestUrl, It.Is<string>(d => string.Equals(d, string.Empty))), Times.Once);
        }

        private static JenkinsJob InitJenkinsJob(List<JenkinsBuild> builds)
        {
            var job = new JenkinsJob
                      {
                          Builds = builds.ToArray(),
                          LastBuild = builds[0],
                          FirstBuild = builds[0],
                          LastCompletedBuild = builds[1],
                          LastFailedBuild = builds[2],
                          LastStableBuild = builds[1],
                          LastSuccessfulBuild = builds[1],
                          LastUnstableBuild = builds[0],
                          LastUnsuccessfulBuild = builds[1],
                      };
            return job;
        }

        private static List<JenkinsBuild> Init3JenkinsBuilds()
        {
            var builds = new List<JenkinsBuild>
                         {
                             new JenkinsBuild {Url = "BuildUrl1"},
                             new JenkinsBuild {Url = "BuildUrl2"},
                             new JenkinsBuild {Url = "BuildUrl3"}
                         };
            return builds;
        }
    }
}