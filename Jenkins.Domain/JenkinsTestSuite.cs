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
    /// Provides an observable Jenkins test suite.
    /// </summary>
    public class JenkinsTestSuite : ObservableObject
    {
        private JenkinsTestCase[] cases;
        private TimeSpan? duration;
        private string id;
        private string name;
        private DateTime? timeStamp;

        [JsonProperty("cases")]
        public JenkinsTestCase[] Cases
        {
            get { return this.cases; }
            set
            {
                if (this.cases != value)
                {
                    this.cases = value;
                    this.RaisePropertyChanged(() => this.Cases);
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
    }
}