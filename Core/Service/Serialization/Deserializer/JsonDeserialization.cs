using System;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace Tests.Helpers.Api.Serialization.Deserializer
{
    /// <summary>
    /// A strategy class for converting from data in a JSON stream to an object.
    /// </summary>
    public class JsonDeserialization : IDeserializer
    {
        /// <summary>
        /// Internal object of JSON deserialization
        /// </summary>
        private static JsonDeserialization _jsonDeserializer;

        /// <summary>
        /// Serializer settings
        /// </summary>
        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Prevents a default instance of the <see cref="JsonDeserialization"/> class from being created.
        /// </summary>
        private JsonDeserialization()
        {
            _settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
        }

        /// <summary>
        /// Gets an instance of JSON convertor
        /// </summary>
        public static JsonDeserialization Instance => _jsonDeserializer ?? (_jsonDeserializer = new JsonDeserialization());

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
        /// Converting from JSON to object
        /// </summary>
        /// <typeparam name="T">type for converting</typeparam>
        /// <param name="response">Response as <see cref="IRestResponse"/></param>
        /// <returns>Converted type <see cref="T"/></returns>
        public T Deserialize<T>(IRestResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            return JsonConvert.DeserializeObject<T>(response.Content, _settings);
        }

        /// <summary>
        /// Converting from JSON to object
        /// </summary>
        /// <typeparam name="T">type for converting</typeparam>
        /// <param name="body">Body as string</param>
        /// <returns>Converted type <see cref="T"/></returns>
        public T Deserialize<T>(string body)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            return JsonConvert.DeserializeObject<T>(body, _settings);
        }
    }
}
