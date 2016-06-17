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
    /// Provides an observable Jenkins test report.
    /// </summary>
    public class JenkinsTestReport : ObservableObject
    {
        private TimeSpan? duration;
        private int failCount;
        private bool isEmpty;
        private int passCount;
        private int skipCount;
        private JenkinsTestSuite[] suites;

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

        [JsonProperty("empty")]
        public bool IsEmpty
        {
            get { return this.isEmpty; }
            set
            {
                if (this.isEmpty != value)
                {
                    this.isEmpty = value;
                    this.RaisePropertyChanged(() => this.IsEmpty);
                }
            }
        }

        [JsonProperty("failCount")]
        public int FailCount
        {
            get { return this.failCount; }
            set
            {
                if (this.failCount != value)
                {
                    this.failCount = value;
                    this.RaisePropertyChanged(() => this.FailCount);
                }
            }
        }

        [JsonProperty("passCount")]
        public int PassCount
        {
            get { return this.passCount; }
            set
            {
                if (this.passCount != value)
                {
                    this.passCount = value;
                    this.RaisePropertyChanged(() => this.PassCount);
                }
            }
        }

        [JsonProperty("skipCount")]
        public int SkipCount
        {
            get { return this.skipCount; }
            set
            {
                if (this.skipCount != value)
                {
                    this.skipCount = value;
                    this.RaisePropertyChanged(() => this.SkipCount);
                }
            }
        }

        [JsonProperty("suites")]
        public JenkinsTestSuite[] Suites
        {
            get { return this.suites; }
            set
            {
                if (this.suites != value)
                {
                    this.suites = value;
                    this.RaisePropertyChanged(() => this.Suites);
                }
            }
        }
    }
}