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
    using System.Threading.Tasks;
    using log4net;
    using Newtonsoft.Json;

    internal class RestManager : IRestManager
    {
        private readonly string userName;

        private readonly string apiToken;

        private bool IsSecuredConnection
        {
            get
            {
                return !string.IsNullOrEmpty(userName) && ! string.IsNullOrEmpty(this.apiToken);
            }
        }

        private static readonly ILog Logger = LogManager.GetLogger("Jenkins.Core");

        private readonly IJenkinsWebClient client;

        public RestManager(IJenkinsWebClient webClient)
        {
            if (webClient == null)
            {
                throw new ArgumentNullException("webClient");
            }

            this.client = webClient;
        }

        public RestManager(IJenkinsWebClient webClient, string userName, string apiToken)
            : this(webClient)
        {
            this.userName = userName;
            this.apiToken = apiToken;
        }

        public async Task<T> LoadAsync<T>(string url)
        {
            return IsSecuredConnection 
                ? await this.LoadAsync<T>(url, this.client.DownloadStringTaskAsync(url, userName, this.apiToken)) 
                : await this.LoadAsync<T>(url, this.client.DownloadStringTaskAsync(url));
        }

        private async Task<T> LoadAsync<T>(string url, Task<string> downloadStringTaskAsync)
        {
            Logger.Debug(string.Format("LoadAsync({0})", url));

            T obj = default(T);

            try
            {
                string json = await downloadStringTaskAsync;

                obj = JsonConvert.DeserializeObject<T>(json);
            }
            catch (WebException webEx)
            {
                Logger.Error(string.Format("REST call failed for url ({0}).", url), webEx);
            }

            return obj;
        }
        
        public async Task PostDataAsync(string url, string data)
        {
            if (IsSecuredConnection)
            {
                await this.client.UploadStringTaskAsync(url, data, userName, this.apiToken);
            }
            else
            {
                await this.client.UploadStringTaskAsync(url, data);                
            }
        }
    }
}