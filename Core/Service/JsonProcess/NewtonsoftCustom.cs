
namespace Core.Service.JsonProcess
{
    using System.IO;

    using Newtonsoft.Json;

    using RestSharp.Deserializers;
    using RestSharp.Serializers;

    public interface IJsonSerializer : ISerializer, IDeserializer
    {
    }

    public class NewtonsoftCustom : IJsonSerializer
	{
		private Newtonsoft.Json.JsonSerializer serializer;

		public NewtonsoftCustom(Newtonsoft.Json.JsonSerializer serializer)
        {
            this.serializer = serializer;           
        }

        public string ContentType
        {
            get => "application/json";  // Probably used for Serialization?

            set { }
        }

        public string DateFormat { get; set; }

        public string Namespace { get; set; }

        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    this.serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;

            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return this.serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

		public static NewtonsoftCustom Default
        {
            get
            {
				return new NewtonsoftCustom(new Newtonsoft.Json.JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                }); 
            }
        }
    }

	}

