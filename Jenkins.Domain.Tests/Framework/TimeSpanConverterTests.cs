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
    public class TimeSpanConverterTests
    {
        private JsonConverter converter;

        [SetUp]
        public void InitContext()
        {
            this.converter = new TimeSpanConverter();
        }

        [Test]
        public void InvokeWriteJsonShouldThrowException()
        {
            Assert.Throws<NotImplementedException>(() => this.converter.WriteJson(default(JsonWriter), default(object), default(JsonSerializer)));
        }

        [Test]
        public void InvokeReadJsonWithMillisecondsShouldReturnTimeSpan()
        {
            var ms = 1255;
            var reader = new JsonTextReader(new StringReader(string.Format("{{duration:{0}}}", ms)));

            // read first element (StartElement token type)
            reader.Read();

            // read duration property name
            reader.Read();

            // read duration property value
            reader.Read();

            var objectType = typeof(int);
            var serializer = new JsonSerializer();

            Assert.AreEqual(TimeSpan.FromMilliseconds(ms), this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithoutTimestampShouldReturnNull()
        {
            var reader = new JsonTextReader(new StringReader("{duration:'any string'}"));

            // read first element (StartElement token type)
            reader.Read();

            // read duration property name
            reader.Read();

            // read duration property value
            reader.Read();

            var objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.IsNull(this.converter.ReadJson(reader, objectType, null, serializer));
        }

        [Test]
        public void InvokeReadJsonWithNullTokenTypeShouldReturnNull()
        {
            var reader = new JsonTextReader(new StringReader("{duration:null}"));

            // read first element (StartElement token type)
            reader.Read();

            // read duration property name
            reader.Read();

            // read duration property value
            reader.Read();

            Type objectType = typeof(string);
            var serializer = new JsonSerializer();

            Assert.IsNull(this.converter.ReadJson(reader, objectType, null, serializer));
        }
    }
}