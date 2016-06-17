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
//   Defines the DateTimeConverterTests type.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Jenkins.Domain.Tests.Framework
{
    using System;
    using System.IO;
    using Domain.Framework;
    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class JenkinsJobStatusConverterTests
    {
        private JsonConverter converter;

        [SetUp]
        public void InitContext()
        {
            this.converter = new JenkinsJobStatusConverter();
        }

        [Test]
        public void InvokeWriteJsonShouldThrowException()
        {
            Assert.Throws<NotImplementedException>(() => this.converter.WriteJson(default(JsonWriter), default(object), default(JsonSerializer)));
        }

        [Test]
        public void InvokeReadJsonWithBlueAnimeShouldReturnGreenRunning()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"blue_anime\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsTrue(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Success, status.State);
        }

        [Test]
        public void InvokeReadJsonWithBlueShouldReturnGreen()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"blue\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Success, status.State);
        }

        [Test]
        public void InvokeReadJsonWithYellowAnimeShouldReturnYellowRunning()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"yellow_anime\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsTrue(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Unstable, status.State);
        }

        [Test]
        public void InvokeReadJsonWithYellowShouldReturnYellow()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"yellow\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Unstable, status.State);
        }

        [Test]
        public void InvokeReadJsonWithRedAnimeShouldReturnRedRunning()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"red_anime\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsTrue(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Failed, status.State);
        }

        [Test]
        public void InvokeReadJsonWithRedShouldReturnRed()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"red\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Failed, status.State);
        }

        [Test]
        public void InvokeReadJsonWithAbortedAnimeShouldReturnAbortedRunning()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"aborted_anime\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsTrue(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Aborted, status.State);
        }

        [Test]
        public void InvokeReadJsonWithAbortedShouldReturnAborted()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"aborted\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Aborted, status.State);
        }

        [Test]
        public void InvokeReadJsonWithDisabledAnimeShouldReturnDisabledRunning()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"disabled_anime\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsTrue(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Disabled, status.State);
        }

        [Test]
        public void InvokeReadJsonWithDisabledShouldReturnDisabled()
        {
            var reader = new JsonTextReader(new StringReader("{color:\"disabled\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.Disabled, status.State);
        }

        [Test]
        public void InvokeReadJsonWithAnyStringShouldReturnDefaultStatus()
        {
            var reader = new JsonTextReader(new StringReader(string.Format("{{result:\"{0}\"}}", "any text")));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.None, status.State);
        }

        [Test]
        public void InvokeReadJsonWithNullTokenTypeShouldReturnDefaultStatus()
        {
            var reader = new JsonTextReader(new StringReader("{color:null}"));

            // read first element (StartElement token type)
            reader.Read();

            // read color property name
            reader.Read();

            // read color property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            var status = this.converter.ReadJson(reader, objectType, null, serializer) as JenkinsJobStatus;
            Assert.IsNotNull(status);
            Assert.IsFalse(status.IsRunning);
            Assert.AreEqual(JenkinsJobState.None, status.State);
        }
    }
}