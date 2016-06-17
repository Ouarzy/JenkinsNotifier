// --------------------------------------------------------------------------------------------------------------------
// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// <summary>
//   Defines the JenkinsJobStatusTests type.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Jenkins.Domain.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class JenkinsJobStatusTests
    {
        private const bool IsRunning = true;
        private const JenkinsJobState State = JenkinsJobState.Success;

        [Test]
        public void Invoke_Equals_OnDifferentReferences_WithSameAttributes_ShouldReturnTrue()
        {
            var status1 = new JenkinsJobStatus { IsRunning = IsRunning, State = State };
            var status2 = new JenkinsJobStatus { IsRunning = IsRunning, State = State };

            Assert.AreEqual(status1, status2);
        }

        [Test]
        public void Invoke_Equals_OnDifferentReferences_WithDifferentRunning_ShouldReturnFalse()
        {
            var status1 = new JenkinsJobStatus { IsRunning = IsRunning, State = State };
            var status2 = new JenkinsJobStatus { IsRunning = !IsRunning, State = State };

            Assert.AreNotEqual(status1, status2);
        }

        [Test]
        public void Invoke_Equals_OnDifferentReferences_WithDifferentState_ShouldReturnFalse()
        {
            var state2 = (JenkinsJobState)((((int)State) + 1) % 6);

            var status1 = new JenkinsJobStatus { IsRunning = IsRunning, State = State };
            var status2 = new JenkinsJobStatus { IsRunning = IsRunning, State = state2 };

            Assert.AreNotEqual(status1, status2);
        }

        [Test]
        public void Invoke_Equals_OnSameReferences_ShouldReturnTrue()
        {
            var status1 = new JenkinsJobStatus { IsRunning = IsRunning, State = State };
            var status2 = status1;

            Assert.AreEqual(status1, status2);
        }
    }
}
