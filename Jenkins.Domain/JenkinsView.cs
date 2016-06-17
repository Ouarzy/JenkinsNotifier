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
    /// Provides an observable view data model. JenkinsView contains <see cref="JenkinsJob"/> entities.
    /// </summary>
    public class JenkinsView : ObservableObject
    {
        private string description;
        private JenkinsJob[] jobs;
        private string name;
        private string url;

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        /// <value>
        /// The view name.
        /// </value>
        [JsonProperty("name")]
        public string Name
        {
            get { return this.name; }
            set
            {
                if (name != value)
                {
                    this.name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        /// <summary>
        /// Gets or sets the view description.
        /// </summary>
        /// <value>
        /// The view description.
        /// </value>
        [JsonProperty("description")]
        public string Description
        {
            get { return this.description; }
            set
            {
                if (description != value)
                {
                    this.description = value;
                    this.RaisePropertyChanged(() => this.Description);
                }
            }
        }

        /// <summary>
        /// Gets or sets the view base URL.
        /// </summary>
        /// <value>
        /// The view URL.
        /// </value>
        [JsonProperty("url")]
        public string Url
        {
            get { return this.url; }
            set
            {
                if (url != value)
                {
                    this.url = value;
                    this.RaisePropertyChanged(() => this.Url);
                    this.RaisePropertyChanged(() => this.DetailsUrl);
                }
            }
        }

        /// <summary>
        /// Gets or sets the jobs contained into this <see cref="JenkinsView"/>.
        /// </summary>
        /// <value>
        /// The view jobs.
        /// </value>
        [JsonProperty("jobs")]
        public JenkinsJob[] Jobs
        {
            get { return this.jobs; }
            set
            {
                if (jobs != value)
                {
                    this.jobs = value;
                    this.RaisePropertyChanged(() => this.Jobs);
                }
            }
        }

        /// <summary>
        /// Gets the url used to get more information on view.
        /// </summary>
        /// <value>
        /// The details URL.
        /// </value>
        public string DetailsUrl
        {
            get { return this.Url + "api/json"; }
        }
    }
}