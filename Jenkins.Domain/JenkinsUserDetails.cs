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
    /// Provides an observable user details data model.
    /// </summary>
    public class JenkinsUserDetails : ObservableObject
    {
        private string url;
        private string description;
        private string fullName;
        private string id;
        private JenkinsUserProperty[] properties;

        /// <summary>
        /// Gets or sets the user absolute URL in Jenkins web server.
        /// </summary>
        /// <value>
        /// The user details URL.
        /// </value>
        [JsonProperty("absoluteUrl")]
        public string Url
        {
            get { return this.url; }
            set
            {
                if (this.url != value)
                {
                    this.url = value;
                    this.RaisePropertyChanged(() => this.Url);
                    this.RaisePropertyChanged(() => this.RestUrl);
                }
            }
        }

        /// <summary>
        /// Gets or sets the user full name.
        /// </summary>
        /// <value>
        /// The user full name.
        /// </value>
        [JsonProperty("fullName")]
        public string FullName
        {
            get { return this.fullName; }
            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    this.RaisePropertyChanged(() => this.FullName);
                }
            }
        }

        [JsonProperty("id")]
        public string Id
        {
            get { return this.id; }
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.RaisePropertyChanged(() => this.Description);
                }
            }
        }

        [JsonProperty("property")]
        public JenkinsUserProperty[] Properties
        {
            get { return this.properties; }
            set
            {
                if (this.properties != value)
                {
                    this.properties = value;
                    this.RaisePropertyChanged(() => this.Properties);
                }
            }
        }

        /// <summary>
        /// Gets the url used to get more user information.
        /// </summary>
        /// <value>
        /// The user REST URL.
        /// </value>
        public string RestUrl
        {
            get { return this.Url + "api/json"; }
        }
    }
}