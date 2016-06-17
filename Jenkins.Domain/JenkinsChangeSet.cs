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
    /// Provides an abservable change set data model. Change set contains source controller history linked to build.
    /// </summary>
    public class JenkinsChangeSet : ObservableObject
    {
        private JenkinsChangeSetItem[] items;
        private string sourceControllerKind;

        /// <summary>
        /// Gets or sets the change set items. Each item represents a commit.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        [JsonProperty("items")]
        public JenkinsChangeSetItem[] Items
        {
            get { return this.items; }
            set
            {
                if (this.items != value)
                {
                    this.items = value;
                    this.RaisePropertyChanged(() => this.Items);
                }
            }
        }

        /// <summary>
        /// Gets or sets the source controller kind (e.g.: 'git' or 'svn').
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        [JsonProperty("kind")]
        public string SourceControllerKind
        {
            get { return this.sourceControllerKind; }
            set
            {
                if (this.sourceControllerKind != value)
                {
                    this.sourceControllerKind = value;
                    this.RaisePropertyChanged(() => this.SourceControllerKind);
                }
            }
        }
    }
}