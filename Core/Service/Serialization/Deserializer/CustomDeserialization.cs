using RestSharp;
using RestSharp.Deserializers;

namespace Tests.Helpers.Api.Serialization.Deserializer
{
    /// <summary>
    /// A strategy class for converting from data in an InputStream to an Object.
    /// </summary>
    public class CustomDeserialization
    {
        /// <summary>
        /// Internal converting field from data in an InputStream to an Object for dependency injection
        /// </summary>
        private readonly IDeserializer _converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDeserialization"/> class.
        /// </summary>
        /// <param name="converter">Dependency injection</param>
        public CustomDeserialization(IDeserializer converter)
        {
            _converter = converter;
        }

        /// <summary>
        /// Deserialize the object as <see cref="T"/>
        /// </summary>
        /// <param name="response"><see cref="IRestResponse"/> to deserialize</param>
        /// <typeparam name="T">Type of the value to be returned</typeparam>
        /// <returns name="T">String <see cref="T"/></returns>
        public T Deserialize<T>(IRestResponse response)
        {
            return _converter.Deserialize<T>(response);
        }
    }
}
