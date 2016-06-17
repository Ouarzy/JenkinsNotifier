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
    /// Provides an observable file path. This describe an added, modified or deleted file in a <see cref="JenkinsChangeSetItem"/>.
    /// </summary>
    public class JenkinsPathChange : ObservableObject
    {
        private string changeType;
        private string filePath;

        /// <summary>
        /// Gets or sets the type of change for a file (e.g: 'edit', 'add', 'delete').
        /// </summary>
        /// <value>
        /// The change type.
        /// </value>
        [JsonProperty("editType")]
        public string ChangeType
        {
            get { return this.changeType; }
            set
            {
                if (this.changeType != value)
                {
                    this.changeType = value;
                    this.RaisePropertyChanged(() => this.ChangeType);
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path which change.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [JsonProperty("file")]
        public string FilePath
        {
            get { return this.filePath; }
            set
            {
                if (this.filePath != value)
                {
                    this.filePath = value;
                    this.RaisePropertyChanged(() => this.FilePath);
                }
            }
        }
    }
}