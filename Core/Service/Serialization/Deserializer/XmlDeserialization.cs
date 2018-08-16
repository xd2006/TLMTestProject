using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace Tests.Helpers.Api.Serialization.Deserializer
{
    /// <summary>
    /// A strategy class for converting from data in a XML stream to an object.
    /// </summary>
    public class XmlDeserialization : IDeserializer
    {
        /// <summary>
        /// Internal object of XML deserialization
        /// </summary>
        private static XmlDeserialization _xmlDeserialization;

        /// <summary>
        /// Prevents a default instance of the <see cref="XmlDeserialization"/> class from being created.
        /// </summary>
        private XmlDeserialization()
        {
        }

        /// <summary>
        /// Gets instance of XML convertor
        /// </summary>
        public static XmlDeserialization Instance => _xmlDeserialization ?? (_xmlDeserialization = new XmlDeserialization());

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
        /// Converting from XML to string
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

            var serializer = new DataContractSerializer(typeof(T));

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(response.Content)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
