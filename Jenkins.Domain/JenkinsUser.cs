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
    using System;
    using Framework;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides an observable user data model.
    /// </summary>
    public class JenkinsUser : ObservableObject
    {
        private JenkinsUserDetails details;
        private JenkinsJob job;
        private DateTime? lastActivity;

        /// <summary>
        /// Gets or sets the last date user has interacted with Jenkins.
        /// </summary>
        /// <value>
        /// The user last activity.
        /// </value>
        [JsonProperty("lastChange")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? LastActivity
        {
            get { return this.lastActivity; }
            set
            {
                if (this.lastActivity != value)
                {
                    this.lastActivity = value;
                    this.RaisePropertyChanged(() => this.LastActivity);
                }
            }
        }

        /// <summary>
        /// Gets or sets the default user <see cref="JenkinsJob"/>.
        /// </summary>
        /// <value>
        /// The user defaut job.
        /// </value>
        [JsonProperty("project")]
        public JenkinsJob Job
        {
            get { return this.job; }
            set
            {
                if (this.job != value)
                {
                    this.job = value;
                    this.RaisePropertyChanged(() => this.Job);
                }
            }
        }

        /// <summary>
        /// Gets or sets the user details.
        /// </summary>
        /// <value>
        /// The user details.
        /// </value>
        [JsonProperty("user")]
        public JenkinsUserDetails Details
        {
            get { return this.details; }
            set
            {
                if (this.details != value)
                {
                    this.details = value;
                    this.RaisePropertyChanged(() => this.Details);
                    this.RaisePropertyChanged(() => this.FullName);
                }
            }
        }

        /// <summary>
        /// Gets the user full name. Direct accessor for <c>Details.FullName</c> property path.
        /// </summary>
        /// <value>
        /// The user full name.
        /// </value>
        public string FullName
        {
            get { return this.Details == null ? null : this.Details.FullName; }
        }
    }
}