// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Jenkins.Domain.Framework
{
    using System;
    using Newtonsoft.Json;

    internal class JenkinsJobStatusConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var status = new JenkinsJobStatus();

            if (reader.TokenType != JsonToken.Null)
            {
                var colorStr = serializer.Deserialize<string>(reader);
                switch (colorStr)
                {
                    case "blue":
                    case "blue_anime":
                        status.State = JenkinsJobState.Success;
                        break;

                    case "red":
                    case "red_anime":
                        status.State = JenkinsJobState.Failed;
                        break;

                    case "yellow":
                    case "yellow_anime":
                        status.State = JenkinsJobState.Unstable;
                        break;

                    case "disabled":
                    case "disabled_anime":
                        status.State = JenkinsJobState.Disabled;
                        break;

                    case "aborted":
                    case "aborted_anime":
                        status.State = JenkinsJobState.Aborted;
                        break;
                    
                    default:
                        status.State = JenkinsJobState.None;
                        break;
                }

                status.IsRunning = colorStr.EndsWith("_anime");
            }

            return status;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}