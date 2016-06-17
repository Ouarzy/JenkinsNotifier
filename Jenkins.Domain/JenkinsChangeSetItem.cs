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
    /// Provides an observable change set item (represents a commit).
    /// </summary>
    public class JenkinsChangeSetItem : ObservableObject
    {
        private string[] affectedPaths;
        private JenkinsUserDetails author;
        private JenkinsPathChange[] changes;
        private string comment;
        private string commitId;
        private string id;
        private string message;
        private DateTime? timeStamp;

        /// <summary>
        /// Gets or sets the commit affected paths.
        /// </summary>
        /// <value>
        /// The affected paths.
        /// </value>
        [JsonProperty("affectedPaths")]
        public string[] AffectedPaths
        {
            get { return this.affectedPaths; }
            set
            {
                if (this.affectedPaths != value)
                {
                    this.affectedPaths = value;
                    this.RaisePropertyChanged(() => this.AffectedPaths);
                }
            }
        }

        /// <summary>
        /// Gets or sets the commit identifier (often represented as a GUID).
        /// </summary>
        /// <value>
        /// The commit identifier.
        /// </value>
        [JsonProperty("commitId")]
        public string CommitId
        {
            get { return this.commitId; }
            set
            {
                if (this.commitId != value)
                {
                    this.commitId = value;
                    this.RaisePropertyChanged(() => this.CommitId);
                }
            }
        }

        /// <summary>
        /// Gets or sets the commit timestamp.
        /// </summary>
        /// <value>
        /// The time stamp.
        /// </value>
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

        /// <summary>
        /// Gets or sets the commit author.
        /// </summary>
        /// <value>
        /// The author.
        /// </value>
        [JsonProperty("author")]
        public JenkinsUserDetails Author
        {
            get { return this.author; }
            set
            {
                if (this.author != value)
                {
                    this.author = value;
                    this.RaisePropertyChanged(() => this.Author);
                }
            }
        }

        /// <summary>
        /// Gets or sets the comment associated to commit.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [JsonProperty("comment")]
        public string Comment
        {
            get { return this.comment; }
            set
            {
                if (this.comment != value)
                {
                    this.comment = value;
                    this.RaisePropertyChanged(() => this.Comment);
                }
            }
        }

        /// <summary>
        /// Gets or sets the change set item identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
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

        /// <summary>
        /// Gets or sets the commit message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty("msg")]
        public string Message
        {
            get { return this.message; }
            set
            {
                if (this.message != value)
                {
                    this.message = value;
                    this.RaisePropertyChanged(() => this.Message);
                }
            }
        }

        /// <summary>
        /// Gets or sets the commit file paths which changed.
        /// </summary>
        /// <value>
        /// The changes.
        /// </value>
        [JsonProperty("paths")]
        public JenkinsPathChange[] Changes
        {
            get { return this.changes; }
            set
            {
                if (this.changes != value)
                {
                    this.changes = value;
                    this.RaisePropertyChanged(() => this.Changes);
                }
            }
        }
    }
}