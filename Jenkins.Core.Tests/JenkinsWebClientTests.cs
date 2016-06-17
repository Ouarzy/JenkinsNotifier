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

    using NUnit.Framework;

    [TestFixture]
    public class JenkinsWebClientTests
    {
        private const string Url = "mon url";
        private const string UserName = "mon username";
        private const string ApiToken = "mon api token";
        private const string Data = "mes datas";

        private JenkinsWebClient client;

        [SetUp]
        public void InitContext()
        {
            this.client = new JenkinsWebClient();
        }

        [Test]
        public void InvokeDownloadStringTaskAsyncWithNullUrlShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.DownloadStringTaskAsync(null));
        }

        [Test]
        public void InvokeDownloadStringTaskAsyncSecuredWithNullUrlShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.DownloadStringTaskAsync(null, UserName, ApiToken));
        }

        [Test]
        public void InvokeDownloadStringTaskAsyncSecuredWithNullUserShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.DownloadStringTaskAsync(Url, null, ApiToken));
        }

        [Test]
        public void InvokeDownloadStringTaskAsyncSecuredWithNullTokenShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.DownloadStringTaskAsync(Url, UserName, null));
        }

        [Test]
        public void InvokeUploadStringTaskAsyncWithNullUrlShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.UploadStringTaskAsync(null, Data));
        }

        [Test]
        public void InvokeUploadStringTaskAsyncWithNullDataShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.UploadStringTaskAsync(Url, null));
        }

        [Test]
        public void InvokeUploadStringTaskAsyncSecuredWithNullDataShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.UploadStringTaskAsync(null, Data, UserName, ApiToken));
        }

        [Test]
        public void InvokeUploadStringTaskAsyncSecuredWithNullUserShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.UploadStringTaskAsync(Url, Data, null, ApiToken));
        }

        [Test]
        public void InvokeUploadStringTaskAsyncSecuredWithNullTokenShouldFail()
        {
            Assert.Throws<ArgumentNullException>(() => this.client.UploadStringTaskAsync(Url, Data, UserName, null));
        }
    }
}