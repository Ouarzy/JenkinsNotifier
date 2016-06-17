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
    /// Provides an observable Jenkins server node.
    /// </summary>
    public class JenkinsNode : ObservableObject
    {
        private string description;
        private bool isQuietingDown;
        private JenkinsJob[] jobs;
        private string mode;
        private string nodeDescription;
        private string nodeName;
        private int numExecutors;
        private JenkinsView primaryJenkinsView;
        private int slaveAgentPort;
        private bool useCrumbs;
        private bool useSecurity;
        private JenkinsView[] jenkinsViews;

        [JsonProperty("mode")]
        public string Mode
        {
            get { return this.mode; }
            set
            {
                if (this.mode != value)
                {
                    this.mode = value;
                    this.RaisePropertyChanged(() => this.Mode);
                }
            }
        }

        [JsonProperty("slaveAgentPort")]
        public int SlaveAgentPort
        {
            get { return this.slaveAgentPort; }
            set
            {
                if (this.slaveAgentPort != value)
                {
                    this.slaveAgentPort = value;
                    this.RaisePropertyChanged(() => this.SlaveAgentPort);
                }
            }
        }

        [JsonProperty("nodeDescription")]
        public string NodeDescription
        {
            get { return this.nodeDescription; }
            set
            {
                if (this.nodeDescription != value)
                {
                    this.nodeDescription = value;
                    this.RaisePropertyChanged(() => this.NodeDescription);
                }
            }
        }

        [JsonProperty("nodeName")]
        public string NodeName
        {
            get { return this.nodeName; }
            set
            {
                if (this.nodeName != value)
                {
                    this.nodeName = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }

        [JsonProperty("numExecutors")]
        public int NumExecutors
        {
            get { return this.numExecutors; }
            set
            {
                if (this.numExecutors != value)
                {
                    this.numExecutors = value;
                    this.RaisePropertyChanged(() => this.NumExecutors);
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

        [JsonProperty("jobs")]
        public JenkinsJob[] Jobs
        {
            get { return this.jobs; }
            set
            {
                if (this.jobs != value)
                {
                    this.jobs = value;
                    if (this.jobs != null)
                    {
                        foreach (JenkinsJob job in this.jobs)
                        {
                            job.Node = this;
                        }
                    }

                    this.RaisePropertyChanged(() => this.Jobs);
                }
            }
        }

        [JsonProperty("quietingdown")]
        public bool IsQuietingDown
        {
            get { return this.isQuietingDown; }
            set
            {
                if (this.isQuietingDown != value)
                {
                    this.isQuietingDown = value;
                    this.RaisePropertyChanged(() => this.IsQuietingDown);
                }
            }
        }

        [JsonProperty("primaryJenkinsView")]
        public JenkinsView PrimaryJenkinsView
        {
            get { return this.primaryJenkinsView; }
            set
            {
                if (this.primaryJenkinsView != value)
                {
                    this.primaryJenkinsView = value;
                    this.RaisePropertyChanged(() => this.PrimaryJenkinsView);
                }
            }
        }

        [JsonProperty("useSecurity")]
        public bool UseSecurity
        {
            get { return this.useSecurity; }
            set
            {
                if (this.useSecurity != value)
                {
                    this.useSecurity = value;
                    this.RaisePropertyChanged(() => this.UseSecurity);
                }
            }
        }

        [JsonProperty("useCrumbs")]
        public bool UseCrumbs
        {
            get { return this.useCrumbs; }
            set
            {
                if (this.useCrumbs != value)
                {
                    this.useCrumbs = value;
                    this.RaisePropertyChanged(() => this.UseCrumbs);
                }
            }
        }

        [JsonProperty("jenkinsViews")]
        public JenkinsView[] JenkinsViews
        {
            get { return this.jenkinsViews; }
            set
            {
                if (this.jenkinsViews != value)
                {
                    this.jenkinsViews = value;
                    this.RaisePropertyChanged(() => this.JenkinsViews);
                }
            }
        }
    }
}