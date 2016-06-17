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
    public class EnumConverterTests
    {
        private JsonConverter converter;

        [SetUp]
        public void InitContext()
        {
            this.converter = new EnumConverter<JenkinsBuildStatus>();
        }

        [Test]
        public void InvokeWriteJsonShouldThrowException()
        {
            Assert.Throws<NotImplementedException>(() => this.converter.WriteJson(default(JsonWriter), default(object), default(JsonSerializer)));
        }

        [Test]
        public void InvokeReadJsonWithSuccessStringShouldReturnEnum()
        {
            var reader = new JsonTextReader(new StringReader("{result:\"SUCCESS\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read result property name
            reader.Read();

            // read result property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.AreEqual(JenkinsBuildStatus.Success, this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithFailureStringShouldReturnEnum()
        {
            var reader = new JsonTextReader(new StringReader("{result:\"FAILURE\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read result property name
            reader.Read();

            // read result property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.AreEqual(JenkinsBuildStatus.Failure, this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithUnstableStringShouldReturnEnum()
        {
            var reader = new JsonTextReader(new StringReader("{result:\"UNSTABLE\"}"));

            // read first element (StartElement token type)
            reader.Read();

            // read result property name
            reader.Read();

            // read result property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.AreEqual(JenkinsBuildStatus.Unstable, this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithAnyStringShouldReturnEnumNone()
        {
            var reader = new JsonTextReader(new StringReader(string.Format("{{result:\"{0}\"}}", "anyText")));

            // read first element (StartElement token type)
            reader.Read();

            // read result property name
            reader.Read();

            // read result property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.AreEqual(JenkinsBuildStatus.None, this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithNullTokenTypeShouldReturnEnumNone()
        {
            var reader = new JsonTextReader(new StringReader("{result:null}"));

            // read first element (StartElement token type)
            reader.Read();

            // read result property name
            reader.Read();

            // read result property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.AreEqual(JenkinsBuildStatus.None, this.converter.ReadJson(reader, objectType, null, serializer));
        }
    }
}