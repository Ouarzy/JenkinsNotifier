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
    using Jenkins.Domain.Framework;
    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class DateTimeConverterTests
    {
        private JsonConverter converter;

        [SetUp]
        public void InitContext()
        {
            this.converter = new DateTimeConverter();
        }

        [Test]
        public void InvokeWriteJsonShouldThrowException()
        {
            Assert.Throws<NotImplementedException>(() => this.converter.WriteJson(default(JsonWriter), default(object), default(JsonSerializer)));
        }

        [Test]
        public void InvokeReadJsonWithTimestampShouldReturnDateTime()
        {
            const int Timestamp = 854564564;
            var reader = new JsonTextReader(new StringReader(string.Format("{{timestamp:{0}}}", Timestamp)));

            // read first element (StartElement token type)
            reader.Read();

            // read timestamp property name
            reader.Read();

            // read timestamp property value
            reader.Read();

            var objectType = typeof(int);
            var serializer = new JsonSerializer();

            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Timestamp).ToLocalTime(), this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithoutTimestampShouldReturnNull()
        {
            var reader = new JsonTextReader(new StringReader("{timestamp:'any string'}"));

            // read first element (StartElement token type)
            reader.Read();

            // read timestamp property name
            reader.Read();

            // read timestamp property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.IsNull(this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithNullTokenTypeShouldReturnNull()
        {
            var reader = new JsonTextReader(new StringReader("{timestamp:null}"));

            // read first element (StartElement token type)
            reader.Read();

            // read timestamp property name
            reader.Read();

            // read timestamp property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.IsNull(this.converter.ReadJson(reader, objectType, null, serializer));
        }
    }
}