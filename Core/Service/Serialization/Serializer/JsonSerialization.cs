using System;
using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;

namespace Tests.Helpers.Api.Serialization.Serializer
{
    /// <summary>
    /// A strategy class for converting from data in an object to a JSON.
    /// </summary>
    public class JsonSerialization : ISerializer
    {
        /// <summary>
        /// Internal object of JSON serialization
        /// </summary>
        private static JsonSerialization _jsonSerializer;

        /// <summary>
        /// Internal object of serialization
        /// </summary>
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        /// <summary>
        /// Prevents a default instance of the <see cref="JsonSerialization"/> class from being created.
        /// </summary>
        private JsonSerialization()
        {
            ContentType = "application/json";
            _serializer = new Newtonsoft.Json.JsonSerializer
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        /// <summary>
        /// Gets instance of JSON convertor
        /// </summary>
        public static JsonSerialization Instance => _jsonSerializer ?? (_jsonSerializer = new JsonSerialization());

        /// <summary>
        /// Gets or sets root element
        /// </summary>
        public string RootElement { get; set; }

        /// <summary>
        /// Gets or sets namespace
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets a date format
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Gets or sets content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Serialize the object as JSON
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>JSON as String</returns>
        public string Serialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            using (StringWriter stringWriter = new StringWriter())
            {
                var jsonTextWriter = new JsonTextWriter(stringWriter)
                {
                    Formatting = Formatting.Indented,
                    QuoteChar = '"'
                };
                _serializer.Serialize(jsonTextWriter, obj);
                return stringWriter.ToString();
            }
        }
    }
}
