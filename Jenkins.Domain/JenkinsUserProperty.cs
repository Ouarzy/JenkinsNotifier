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
    /// Provides an observable Jenkins user property.
    /// </summary>
    public class JenkinsUserProperty : ObservableObject
    {
        private string address;
        private bool hasInsensitiveSearch;

        [JsonProperty("address")]
        public string Address
        {
            get { return this.address; }
            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    this.RaisePropertyChanged(() => this.Address);
                }
            }
        }

        [JsonProperty("insensitiveSearch")]
        public bool HasInsensitiveSearch
        {
            get { return this.hasInsensitiveSearch; }
            set
            {
                if (this.hasInsensitiveSearch != value)
                {
                    this.hasInsensitiveSearch = value;
                    this.RaisePropertyChanged(() => this.HasInsensitiveSearch);
                }
            }
        }
    }
}