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
    /// Provides an observable Jenkins test case.
    /// </summary>
    public class JenkinsTestCase : ObservableObject
    {
        private int age;
        private string className;
        private TimeSpan? duration;
        private string errorDetails;
        private string errorStackTrace;
        private int failedSince;
        private bool isSkipped;
        private string name;
        private string skippedMessage;
        private JenkinsTestStatus status;

        [JsonProperty("age")]
        public int Age
        {
            get { return this.age; }
            set
            {
                if (this.age != value)
                {
                    this.age = value;
                    this.RaisePropertyChanged(() => this.Age);
                }
            }
        }

        [JsonProperty("className")]
        public string ClassName
        {
            get { return this.className; }
            set
            {
                if (this.className != value)
                {
                    this.className = value;
                    this.RaisePropertyChanged(() => this.ClassName);
                }
            }
        }

        [JsonProperty("duration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan? Duration
        {
            get { return this.duration; }
            set
            {
                if (this.duration != value)
                {
                    this.duration = value;
                    this.RaisePropertyChanged(() => this.Duration);
                }
            }
        }

        [JsonProperty("errorDetails")]
        public string ErrorDetails
        {
            get { return this.errorDetails; }
            set
            {
                if (this.errorDetails != value)
                {
                    this.errorDetails = value;
                    this.RaisePropertyChanged(() => this.ErrorDetails);
                }
            }
        }

        [JsonProperty("errorStackTrace")]
        public string ErrorStackTrace
        {
            get { return this.errorStackTrace; }
            set
            {
                if (this.errorStackTrace != value)
                {
                    this.errorStackTrace = value;
                    this.RaisePropertyChanged(() => this.ErrorStackTrace);
                }
            }
        }

        /// <summary>
        /// Gets or sets the build number since this test case failed.
        /// </summary>
        /// <value>
        /// The build number since this test case failed.
        /// </value>
        [JsonProperty("failedSince")]
        public int FailedSince
        {
            get { return this.failedSince; }
            set
            {
                if (this.failedSince != value)
                {
                    this.failedSince = value;
                    this.RaisePropertyChanged(() => this.FailedSince);
                }
            }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        [JsonProperty("skipped")]
        public bool IsSkipped
        {
            get { return this.isSkipped; }
            set
            {
                if (this.isSkipped != value)
                {
                    this.isSkipped = value;
                    this.RaisePropertyChanged(() => this.IsSkipped);
                }
            }
        }

        [JsonProperty("skippedMessage")]
        public string SkippedMessage
        {
            get { return this.skippedMessage; }
            set
            {
                if (this.skippedMessage != value)
                {
                    this.skippedMessage = value;
                    this.RaisePropertyChanged(() => this.SkippedMessage);
                }
            }
        }

        [JsonProperty("status")]
        [JsonConverter(typeof(EnumConverter<JenkinsTestStatus>))]
        public JenkinsTestStatus Status
        {
            get { return this.status; }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.RaisePropertyChanged(() => this.Status);
                }
            }
        }
    }
}