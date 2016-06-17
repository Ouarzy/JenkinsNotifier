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
    /// Provides an observable artifact data model. Artifacts are downloadables linked to a build.
    /// </summary>
    public class JenkinsArtifact : ObservableObject
    {
        private string displayPath;
        private string fileName;
        private string relativePath;

        /// <summary>
        /// Gets or sets the file path as is should be displayed to user.
        /// </summary>
        /// <value>
        /// The display path.
        /// </value>
        [JsonProperty("displayPath")]
        public string DisplayPath
        {
            get { return this.displayPath; }
            set
            {
                if (this.displayPath != value)
                {
                    this.displayPath = value;
                    this.RaisePropertyChanged(() => this.DisplayPath);
                }
            }
        }

        /// <summary>
        /// Gets or sets the real file name without access path.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [JsonProperty("fileName")]
        public string FileName
        {
            get { return this.fileName; }
            set
            {
                if (this.fileName != value)
                {
                    this.fileName = value;
                    this.RaisePropertyChanged(() => this.FileName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the file relative path, based on current build target/ url.
        /// </summary>
        /// <value>
        /// The relative path.
        /// </value>
        [JsonProperty("relativePath")]
        public string RelativePath
        {
            get { return this.relativePath; }
            set
            {
                if (this.relativePath != value)
                {
                    this.relativePath = value;
                    this.RaisePropertyChanged(() => this.RelativePath);
                }
            }
        }
    }
}