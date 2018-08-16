using System;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using RestSharp.Serializers;

namespace Tests.Helpers.Api.Serialization.Serializer
{
    /// <summary>
    /// A strategy class for converting from data in an object to a XML.
    /// </summary>
    public class XmlSerialization : ISerializer
    {
        /// <summary>
        /// Internal object of XML serialization
        /// </summary>
        private static XmlSerialization _xmlSerialization;

        /// <summary>
        /// Prevents a default instance of the <see cref="XmlSerialization"/> class from being created.
        /// </summary>
        private XmlSerialization()
        {
            ContentType = "application/xml";
            DateFormat = RestSharp.DateFormat.ROUND_TRIP;
        }

        /// <summary>
        /// Gets instance of XML convertor
        /// </summary>
        public static XmlSerialization Instance => _xmlSerialization ?? (_xmlSerialization = new XmlSerialization());

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
        /// Serialize the object as XML
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>XML as String</returns>
        public string Serialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                new DataContractSerializer(obj.GetType()).WriteObject(writer, obj);
            }

            return sb.ToString();
        }
    }
}
