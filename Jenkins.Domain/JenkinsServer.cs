// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Jenkins.Domain
{
    using Newtonsoft.Json;

    /// <summary>
    /// Provides an observable Jenkins server data model. This is not a Jenkins data model class.
    /// </summary>
    public class JenkinsServer : ObservableObject
    {
        private JenkinsNode node;
        private string url;
        private JenkinsUser[] users;

        /// <summary>
        /// Gets or sets the server root URL.
        /// </summary>
        /// <value>
        /// The server root URL.
        /// </value>
        public string Url
        {
            get { return this.url; }
            set
            {
                if (this.url != value)
                {
                    this.url = value;
                    this.RaisePropertyChanged(() => this.Url);
                }
            }
        }

        private string BaseUrl
        {
            get { return this.url + (this.url.EndsWith("/") ? string.Empty : "/"); }
        }

        /// <summary>
        /// Gets the main REST URL. This is the Jenkins main page data.
        /// </summary>
        /// <value>
        /// The main REST URL.
        /// </value>
        public string NodeRestUrl
        {
            get { return this.BaseUrl + "api/json"; }
        }

        /// <summary>
        /// Gets the users REST URL. This is the Jenkins People page data.
        /// </summary>
        /// <value>
        /// The users REST URL.
        /// </value>
        public string UsersRestUrl
        {
            get { return this.BaseUrl + "asynchPeople/api/json"; }
        }

        /// <summary>
        /// Gets or sets the Jenkins server node.
        /// </summary>
        /// <value>
        /// The server node.
        /// </value>
        public JenkinsNode Node
        {
            get { return this.node; }
            set
            {
                if (this.node != value)
                {
                    this.node = value;
                    this.RaisePropertyChanged(() => this.Node);
                }
            }
        }

        /// <summary>
        /// Gets or sets the server users.
        /// </summary>
        /// <value>
        /// The server users.
        /// </value>
        [JsonProperty("users")]
        public JenkinsUser[] Users
        {
            get { return this.users; }
            set
            {
                if (this.users != value)
                {
                    this.users = value;
                    this.RaisePropertyChanged(() => this.Users);
                }
            }
        }
    }
}