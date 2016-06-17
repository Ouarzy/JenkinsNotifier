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
    using Framework;
    using Newtonsoft.Json;

    /// <summary>
    /// Provides an observable Jenkins job.
    /// </summary>
    public class JenkinsJob : ObservableObject
    {
        private JenkinsJobAction[] actions;
        private bool buildable;
        private JenkinsBuild[] builds;
        private string description;
        private string displayName;

        private JenkinsBuild firstBuild;
        private JenkinsHealthReport[] healthReports;
        private bool isConcurrentBuild;
        private bool isInQueue;
        private bool keepDependencies;
        private JenkinsBuild lastBuild;
        private JenkinsBuild lastCompletedBuild;
        private JenkinsBuild lastFailedBuild;
        private JenkinsBuild lastStableBuild;
        private JenkinsBuild lastSuccessfulBuild;
        private JenkinsBuild lastUnstableBuild;
        private JenkinsBuild lastUnsuccessfulBuild;
        private string name;
        private int nextBuildNumber;
        private JenkinsNode node;
        private JenkinsJobProperty[] properties;
        private JenkinsJobStatus status;
        private string url;

        [JsonProperty("actions")]
        public JenkinsJobAction[] Actions
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

        [JsonProperty("concurrentBuild")]
        public bool IsConcurrentBuild
        {
            get { return this.isConcurrentBuild; }
            set
            {
                if (this.isConcurrentBuild != value)
                {
                    this.isConcurrentBuild = value;
                    this.RaisePropertyChanged(() => this.IsConcurrentBuild);
                }
            }
        }

        [JsonProperty("property")]
        public JenkinsJobProperty[] Properties
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

        [JsonProperty("keepDependencies")]
        public bool KeepDependencies
        {
            get { return this.keepDependencies; }
            set
            {
                if (this.keepDependencies != value)
                {
                    this.keepDependencies = value;
                    this.RaisePropertyChanged(() => this.KeepDependencies);
                }
            }
        }

        [JsonProperty("healthReport")]
        public JenkinsHealthReport[] HealthReports
        {
            get { return this.healthReports; }
            set
            {
                if (this.healthReports != value)
                {
                    this.healthReports = value;
                    this.RaisePropertyChanged(() => this.HealthReports);
                }
            }
        }

        /// <summary>
        /// Gets or sets the job name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
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

        /// <summary>
        /// Gets or sets the job base url.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
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
                    this.RaisePropertyChanged(() => this.BuildRestUrl);
                    this.RaisePropertyChanged(() => this.RestUrl);
                }
            }
        }

        /// <summary>
        /// Gets or sets the job user-friendly name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [JsonProperty("displayName")]
        public string DisplayName
        {
            get { return this.displayName ?? this.Name; }
            set
            {
                if (this.displayName != value)
                {
                    this.displayName = value;
                    this.RaisePropertyChanged(() => this.DisplayName);
                }
            }
        }

        [JsonProperty("firstBuild")]
        public JenkinsBuild FirstBuild
        {
            get { return this.firstBuild; }
            set
            {
                if (this.firstBuild != value)
                {
                    this.firstBuild = value;
                    this.RaisePropertyChanged(() => this.FirstBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the job description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JenkinsJob"/> is buildable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if buildable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("buildable")]
        public bool Buildable
        {
            get { return this.buildable; }
            set
            {
                if (this.buildable != value)
                {
                    this.buildable = value;
                    this.RaisePropertyChanged(() => this.Buildable);
                }
            }
        }

        /// <summary>
        /// Gets or sets the job build history.
        /// </summary>
        /// <value>
        /// The builds.
        /// </value>
        [JsonProperty("builds")]
        public JenkinsBuild[] Builds
        {
            get { return this.builds; }
            set
            {
                if (this.builds != value)
                {
                    this.builds = value;
                    if (this.builds != null)
                    {
                        foreach (JenkinsBuild build in this.builds)
                        {
                            build.Job = this;
                        }
                    }

                    this.RaisePropertyChanged(() => this.Builds);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JenkinsJob"/> is in queue.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="JenkinsJob"/> is in queue; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("inQueue")]
        public bool IsInQueue
        {
            get { return this.isInQueue; }
            set
            {
                if (this.isInQueue != value)
                {
                    this.isInQueue = value;
                    this.RaisePropertyChanged(() => this.IsInQueue);
                }
            }
        }

        /// <summary>
        /// Gets or sets the job status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [JsonProperty("color")]
        [JsonConverter(typeof(JenkinsJobStatusConverter))]
        public JenkinsJobStatus Status
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

        /// <summary>
        /// Gets or sets the last build for this job.
        /// </summary>
        /// <value>
        /// The last build.
        /// </value>
        [JsonProperty("lastBuild")]
        public JenkinsBuild LastBuild
        {
            get { return this.lastBuild; }
            set
            {
                if (this.lastBuild != value)
                {
                    this.lastBuild = value;
                    this.RaisePropertyChanged(() => this.LastBuild);
                }
            }
        }

        /// <summary>
        /// Gets the parent <see cref="JenkinsNode" /> this job belong to when attached to job using <see cref="JenkinsNode.Jobs"/> setter.
        /// </summary>
        /// <value>
        /// The parent node.
        /// </value>
        public JenkinsNode Node
        {
            get { return this.node; }
            internal set
            {
                if (this.node != value)
                {
                    this.node = value;
                    this.RaisePropertyChanged(() => this.Node);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last completed build for this job.
        /// </summary>
        /// <value>
        /// The last completed build.
        /// </value>
        [JsonProperty("lastCompletedBuild")]
        public JenkinsBuild LastCompletedBuild
        {
            get { return this.lastCompletedBuild; }
            set
            {
                if (this.lastCompletedBuild != value)
                {
                    this.lastCompletedBuild = value;
                    this.RaisePropertyChanged(() => this.LastCompletedBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last failed build for this job.
        /// </summary>
        /// <value>
        /// The last failed build.
        /// </value>
        [JsonProperty("lastFailedBuild")]
        public JenkinsBuild LastFailedBuild
        {
            get { return this.lastFailedBuild; }
            set
            {
                if (this.lastFailedBuild != value)
                {
                    this.lastFailedBuild = value;
                    this.RaisePropertyChanged(() => this.LastFailedBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last stable build for this job.
        /// </summary>
        /// <value>
        /// The last stable build.
        /// </value>
        [JsonProperty("lastStableBuild")]
        public JenkinsBuild LastStableBuild
        {
            get { return this.lastStableBuild; }
            set
            {
                if (this.lastStableBuild != value)
                {
                    this.lastStableBuild = value;
                    this.RaisePropertyChanged(() => this.LastStableBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last successful build for this job.
        /// </summary>
        /// <value>
        /// The last successful build.
        /// </value>
        [JsonProperty("lastSuccessfulBuild")]
        public JenkinsBuild LastSuccessfulBuild
        {
            get { return this.lastSuccessfulBuild; }
            set
            {
                if (this.lastSuccessfulBuild != value)
                {
                    this.lastSuccessfulBuild = value;
                    this.RaisePropertyChanged(() => this.LastSuccessfulBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last unstable build for this job.
        /// </summary>
        /// <value>
        /// The last unstable build.
        /// </value>
        [JsonProperty("lastUnstableBuild")]
        public JenkinsBuild LastUnstableBuild
        {
            get { return this.lastUnstableBuild; }
            set
            {
                if (this.lastUnstableBuild != value)
                {
                    this.lastUnstableBuild = value;
                    this.RaisePropertyChanged(() => this.LastUnstableBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the last unsuccessful build for this job.
        /// </summary>
        /// <value>
        /// The last unsuccessful build.
        /// </value>
        [JsonProperty("lastUnsuccessfulBuild")]
        public JenkinsBuild LastUnsuccessfulBuild
        {
            get { return this.lastUnsuccessfulBuild; }
            set
            {
                if (this.lastUnsuccessfulBuild != value)
                {
                    this.lastUnsuccessfulBuild = value;
                    this.RaisePropertyChanged(() => this.LastUnsuccessfulBuild);
                }
            }
        }

        /// <summary>
        /// Gets or sets the next build number for this job.
        /// </summary>
        /// <value>
        /// The next build number.
        /// </value>
        [JsonProperty("nextBuildNumber")]
        public int NextBuildNumber
        {
            get { return this.nextBuildNumber; }
            set
            {
                if (this.nextBuildNumber != value)
                {
                    this.nextBuildNumber = value;
                    this.RaisePropertyChanged(() => this.NextBuildNumber);
                }
            }
        }

        /// <summary>
        /// Gets the url used to start a job building.
        /// </summary>
        /// <value>
        /// The job build URL.
        /// </value>
        public string BuildRestUrl
        {
            get { return this.Url + "build"; }
        }

        /// <summary>
        /// Gets the url used to get more job information.
        /// </summary>
        /// <value>
        /// The job REST URL.
        /// </value>
        public string RestUrl
        {
            get { return this.Url + "api/json"; }
        }
    }
}