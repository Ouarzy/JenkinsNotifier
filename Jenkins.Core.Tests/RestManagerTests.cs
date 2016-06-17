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
namespace Jenkins.Core.Tests
{
    using System;
    using System.Threading.Tasks;
    using Moq;
    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class RestManagerTests
    {
        private const string User = "toto";
        private const string ApiToken = "fc8257b069547e32f01d48ad074df200";
        private const string Url = "mon URL";
        private static readonly SomeObject Response = new SomeObject("my object", DateTime.Today);

        private RestManager defaultManager;
        private RestManager defaultSecuredManager;
        private Mock<IJenkinsWebClient> webClient;

        [SetUp]
        public void InitContext()
        {
            this.webClient = new Mock<IJenkinsWebClient>();
            this.defaultManager = new RestManager(this.webClient.Object);
            this.defaultSecuredManager = new RestManager(this.webClient.Object, User, ApiToken);
        }

        [Test]
        public void InvokeConstructorWithNullWebClientShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => new RestManager(null));
        }

        [Test]
        public void InvokeLoadAsyncReturnResponse()
        {
            this.webClient.Setup(c => c.DownloadStringTaskAsync(It.IsAny<string>()))
                .Returns((string u) =>
                {
                    var tcs = new TaskCompletionSource<string>();
                    tcs.SetResult(JsonConvert.SerializeObject(Response));
                    return tcs.Task;
                });

            SomeObject result = this.defaultManager.LoadAsync<SomeObject>(Url).Result;

            this.webClient.Verify(c => c.DownloadStringTaskAsync(It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase))), Times.Once);

            Assert.AreEqual(Response, result);
        }

        [Test]
        public void InvokeLoadAsyncWithCredentialReturnResponse()
        {
            this.webClient.Setup(c => c.DownloadStringTaskAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string x, string y, string z) =>
                {
                    var tcs = new TaskCompletionSource<string>();
                    tcs.SetResult(JsonConvert.SerializeObject(Response));
                    return tcs.Task;
                });

            var result = this.defaultSecuredManager.LoadAsync<SomeObject>(Url).Result;

            this.webClient.Verify(
                c => c.DownloadStringTaskAsync(
                    It.Is<string>(u => string.Equals(u, Url, StringComparison.OrdinalIgnoreCase)),
                    It.Is<string>(u => string.Equals(u, User, StringComparison.OrdinalIgnoreCase)),
                It.Is<string>(u => string.Equals(u, ApiToken, StringComparison.OrdinalIgnoreCase))),
                Times.Once);

            Assert.AreEqual(Response, result);
        }

        private struct SomeObject
        {
            public string Name { get; set; }

            public DateTime Date { get; set; }

            public SomeObject(string name, DateTime date)
                : this()
            {
                this.Name = name;
                this.Date = date;
            }
        }
    }
}