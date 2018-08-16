using RestSharp;
using RestSharp.Serializers;

namespace Tests.Helpers.Api.Serialization.Serializer
{
    /// <summary>
    /// A strategy class for converting from data in an object to a desired format.
    /// </summary>
    public class CustomSerialization
    {
        /// <summary>
        /// Data format
        /// </summary>
        private readonly DataFormat _format;

        /// <summary>
        ///  Initializes a new instance of the <see cref="CustomSerialization"/> class.
        /// </summary>
        /// <param name="format">Data format</param>
        public CustomSerialization(DataFormat format)
        {
            _format = format;
        }

        /// <summary>
        /// Gets internal converting field from data in an Object to an Stream for dependency injection
        /// </summary>
        private ISerializer Serializer
        {
            get
            {
                if (_format == DataFormat.Json)
                {
                    return JsonSerialization.Instance;
                }
                return XmlSerialization.Instance;
            }
        }

        /// <summary>
        /// Serialize the object_format
        /// </summary>
        /// <param name="value">Object to serialize</param>
        /// <returns>_format as String</returns>
        public string Serialize(object value)
        {
            return Serializer.Serialize(value);
        }
    }
}
