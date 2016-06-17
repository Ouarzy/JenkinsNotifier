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
    /// <summary>
    /// Provides an observable job descriptive state.
    /// </summary>
    public class JenkinsJobStatus : ObservableObject
    {
        private bool isRunning;
        private JenkinsJobState state;

        /// <summary>
        /// Gets or sets the underlying <see cref="JenkinsJob"/> state.
        /// </summary>
        /// <value>
        /// The state of the underlying job.
        /// </value>
        public JenkinsJobState State
        {
            get { return this.state; }
            set
            {
                this.state = value;
                this.RaisePropertyChanged(() => this.State);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying <see cref="JenkinsJob"/> is running.
        /// </summary>
        /// <value>
        /// <c>true</c> if the underlying <see cref="JenkinsJobStatus"/> is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get { return this.isRunning; }
            set
            {
                this.isRunning = value;
                this.RaisePropertyChanged(() => this.IsRunning);
            }
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(JenkinsJobStatus other)
        {
            return !ReferenceEquals(null, other) && this.state == other.state && this.isRunning.Equals(other.isRunning);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((JenkinsJobStatus)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.state * 397) ^ this.isRunning.GetHashCode();
            }
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(JenkinsJobStatus left, JenkinsJobStatus right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(JenkinsJobStatus left, JenkinsJobStatus right)
        {
            return !Equals(left, right);
        }
    }
}