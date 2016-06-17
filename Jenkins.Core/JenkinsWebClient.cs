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
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    internal class JenkinsWebClient : IJenkinsWebClient
    {
        /// <summary>
        /// Proxy to <see cref="WebClient.DownloadStringTaskAsync(string)"/>. Use one instance of <see cref="WebClient"/> per call because <see cref="WebClient"/> cannot be used cross thread.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        public Task<string> DownloadStringTaskAsync(string url)
        {
            using (var wc = new WebClient())
            {
                return wc.DownloadStringTaskAsync(url);
            }
        }

        /// <summary>
        /// Proxy to <see cref="WebClient.DownloadStringTaskAsync(string)"/>. Use one instance of <see cref="WebClient"/> per call because <see cref="WebClient"/> cannot be used cross thread.
        /// </summary>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="userName">
        /// The user Name.
        /// </param>
        /// <param name="apiToken">
        /// The Jenkins API Token (http://jenkins-ci.361315.n4.nabble.com/how-to-get-a-API-token-for-user-td4696918.html).
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If any parameter is null.
        /// </exception>
        public Task<string> DownloadStringTaskAsync(string url, string userName, string apiToken)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (apiToken == null)
            {
                throw new ArgumentNullException("apiToken");
            }

            using (var wc = new WebClient())
            {
                var credentialBuffer = GenerateCredentialBuffer(userName, apiToken);
                wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                return wc.DownloadStringTaskAsync(new Uri(url));
            }
        }

        /// <summary>
        /// Proxy to <see cref="WebClient.UploadStringTaskAsync(string, string)"/>. Use one instance of <see cref="WebClient"/> per call because <see cref="WebClient"/> cannot be used cross thread.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> or <c>data</c> parameter is null.</exception>
        public Task<string> UploadStringTaskAsync(string url, string data)
        {
            using (var wc = new WebClient())
            {
                return wc.UploadStringTaskAsync(url, data);
            }
        }

        /// <summary>
        /// Proxy to <see cref="WebClient.UploadStringTaskAsync(string, string)"/>. Use one instance of <see cref="WebClient"/> per call because <see cref="WebClient"/> cannot be used cross thread.
        /// </summary>
        /// <param name="url">
        /// The URL.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="userName">
        /// The user Name.
        /// </param>
        /// <param name="apiToken">
        /// The Jenkins API Token (http://jenkins-ci.361315.n4.nabble.com/how-to-get-a-API-token-for-user-td4696918.html).
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If any parameter is null.
        /// </exception>
        public Task<string> UploadStringTaskAsync(string url, string data, string userName, string apiToken)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (apiToken == null)
            {
                throw new ArgumentNullException("apiToken");
            }

            using (var wc = new WebClient())
            {
                var credentialBuffer = GenerateCredentialBuffer(userName, apiToken);
                wc.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);
                return wc.UploadStringTaskAsync(url, data);
            }
        }

        private static byte[] GenerateCredentialBuffer(string userName, string apiToken)
        {
            return new UTF8Encoding().GetBytes(userName + ":" + apiToken);
        }
    }
}