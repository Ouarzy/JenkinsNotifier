// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Jenkins.Core
{
    using System.Threading;

    /// <summary>
    /// Provides methods that give access to Jenkins REST API.
    /// </summary>
    public class JenkinsRestFactory
    {
        private readonly string userName;
        private readonly string apiToken;
        private readonly ReaderWriterLockSlim restClientLock;
        private IJenkinsRestClient restClient;
        private IRestManager restManager;
        private IJenkinsWebClient webClient;

        public JenkinsRestFactory()
        {
            this.restClientLock = new ReaderWriterLockSlim();
        }

        public JenkinsRestFactory(string userName, string apiToken) : this()
        {
            this.userName = userName;
            this.apiToken = apiToken;
        }

        /// <summary>
        /// Provides a Web Client for Jenkins REST API.
        /// </summary>
        /// <returns>A <see cref="IJenkinsRestClient"/> instance.</returns>
        public IJenkinsRestClient GetClient()
        {
            try
            {
                this.restClientLock.EnterUpgradeableReadLock();

                if (this.restClient == null)
                {
                    try
                    {
                        this.restClientLock.EnterWriteLock();

                        this.webClient = new JenkinsWebClient();
                        this.restManager = new RestManager(this.webClient, this.userName, this.apiToken);
                        this.restClient = new JenkinsRestClient(this.restManager);
                    }
                    finally
                    {
                        this.restClientLock.ExitWriteLock();
                    }
                }

                return this.restClient;
            }
            finally
            {
                this.restClientLock.ExitUpgradeableReadLock();
            }
        }
    }
}