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
    /// Provides an observable build data model.
    /// </summary>
    public class JenkinsBuild : ObservableObject
    {
        private JenkinsBuildAction[] actions;
        private JenkinsArtifact[] artifacts;
        private JenkinsChangeSet changeSet;
        private JenkinsUserDetails[] culprits;
        private string displayName;
        private TimeSpan? duration;
        private TimeSpan? estimatedDuration;
        private string id;
        private bool isBuilding;
        private JenkinsJob job;
        private bool keepLog;
        private int number;
        private JenkinsBuildStatus status;
        private JenkinsTestReport testReport;
        private DateTime? timeStamp;
        private string url;

        public JenkinsTestReport TestReport
        {
            get { return this.testReport; }
            set
            {
                if (this.testReport != value)
                {
                    this.testReport = value;
                    this.RaisePropertyChanged(() => this.TestReport);
                }
            }
        }

        [JsonProperty("actions")]
        public JenkinsBuildAction[] Actions
        {
            get { return this.actions; }
            set
            {
                if (this.actions != value)
                {
                    this.actions = value;
                    this.RaisePropertyChanged(() => this.Actions);
                }
            }
        }

        [JsonProperty("number")]
        public int Number
        {
            get { return this.number; }
            set
            {
                if (this.number != value)
                {
                    this.number = value;
                    this.RaisePropertyChanged(() => this.Number);
                }
            }
        }

        [JsonProperty("url")]
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
                    this.RaisePropertyChanged(() => this.StopRestUrl);
                    this.RaisePropertyChanged(() => this.TestRestUrl);
                }
            }
        }

        [JsonProperty("building")]
        public bool IsBuilding
        {
            get { return this.isBuilding; }
            set
            {
                if (this.isBuilding != value)
                {
                    this.isBuilding = value;
                    this.RaisePropertyChanged(() => this.IsBuilding);
                }
            }
        }

        /// <summary>
        /// Gets the parent <see cref="JenkinsJob"/> this build belong to when attached to job using <see cref="JenkinsJob.Builds"/> setter.
        /// </summary>
        /// <value>
        /// The parent job.
        /// </value>
        public JenkinsJob Job
        {
            get { return this.job; }
            internal set
            {
                if (this.job != value)
                {
                    this.job = value;
                    this.RaisePropertyChanged(() => this.Job);
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

        [JsonProperty("estimatedDuration")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan? EstimatedDuration
        {
            get { return this.estimatedDuration; }
            set
            {
                if (this.estimatedDuration != value)
                {
                    this.estimatedDuration = value;
                    this.RaisePropertyChanged(() => this.EstimatedDuration);
                }
            }
        }

        [JsonProperty("fullDisplayName")]
        public string DisplayName
        {
            get { return this.displayName; }
            set
            {
                if (this.displayName != value)
                {
                    this.displayName = value;
                    this.RaisePropertyChanged(() => this.DisplayName);
                }
            }
        }

        [JsonProperty("keepLog")]
        public bool KeepLog
        {
            get { return this.keepLog; }
            set
            {
                if (this.keepLog != value)
                {
                    this.keepLog = value;
                    this.RaisePropertyChanged(() => this.KeepLog);
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

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? TimeStamp
        {
            get { return this.timeStamp; }
            set
            {
                if (this.timeStamp != value)
                {
                    this.timeStamp = value;
                    this.RaisePropertyChanged(() => this.TimeStamp);
                }
            }
        }

        [JsonProperty("result")]
        [JsonConverter(typeof(EnumConverter<JenkinsBuildStatus>))]
        public JenkinsBuildStatus Status
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

        [JsonProperty("changeSet")]
        public JenkinsChangeSet ChangeSet
        {
            get { return this.changeSet; }
            set
            {
                if (this.changeSet != value)
                {
                    this.changeSet = value;
                    this.RaisePropertyChanged(() => this.ChangeSet);
                }
            }
        }

        [JsonProperty("artifacts")]
        public JenkinsArtifact[] Artifacts
        {
            get { return this.artifacts; }
            set
            {
                if (this.artifacts != value)
                {
                    this.artifacts = value;
                    this.RaisePropertyChanged(() => this.Artifacts);
                }
            }
        }

        [JsonProperty("culprits")]
        public JenkinsUserDetails[] Culprits
        {
            get { return this.culprits; }
            set
            {
                if (this.culprits != value)
                {
                    this.culprits = value;
                    this.RaisePropertyChanged(() => this.Culprits);
                }
            }
        }

        /// <summary>
        /// Gets url used to get actual console output text.
        /// </summary>
        /// <value>
        /// The console text URL.
        /// </value>
        public string ConsoleTextUrl
        {
            get { return this.Url + "consoleText"; }
        }

        /// <summary>
        /// Gets the url used to get more information on build.
        /// </summary>
        /// <value>
        /// The REST URL.
        /// </value>
        public string RestUrl
        {
            get { return this.Url + "api/json"; }
        }

        /// <summary>
        /// Gets the url used to stop a build.
        /// </summary>
        /// <value>
        /// The stop REST URL.
        /// </value>
        public string StopRestUrl
        {
            get { return this.Url + "stop"; }
        }

        /// <summary>
        /// Gets the url used to get test results of build.
        /// </summary>
        /// <value>
        /// The test results REST URL.
        /// </value>
        public string TestRestUrl
        {
            get { return this.Url + "testReport/api/json"; }
        }
    }
}