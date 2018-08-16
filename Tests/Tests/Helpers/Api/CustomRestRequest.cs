using System.Linq;
using System.Text;
using RestSharp;
using Tests.Helpers.Api.Serialization.Serializer;

namespace Tests.Helpers.Api
{
    /// <summary>
    /// custom request
    /// </summary>
    public class CustomRestRequest
    {
        /// <summary>
        /// Base internal request object
        /// </summary>
        private readonly RestRequest _request;

        /// <summary>
        /// request information
        /// </summary>
        private readonly StringBuilder _requestInformation = new StringBuilder();

        /// <summary>
        /// Data format
        /// </summary>
        private DataFormat _baseDataFormat = DataFormat.Xml;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomRestRequest"/> class.
        /// </summary>
        public CustomRestRequest()
        {
            _request = new RestRequest()
            {
                JsonSerializer = JsonSerialization.Instance,
                XmlSerializer = XmlSerialization.Instance
            };
        }

        /// <summary>
        /// Gets name of request
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        ///  Adding resource to request
        /// </summary>
        /// <param name="resource">resource value</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithResource(string resource)
        {
            _request.Resource = resource;
            _requestInformation.AppendLine($"Resource: {resource}");
            return this;
        }

        /// <summary>
        /// Adding method type to request
        /// </summary>
        /// <param name="method">adding method as <see cref="Method"/></param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithMethod(Method method)
        {
            _request.Method = method;
            _requestInformation.AppendLine($"Method: {method}");
            return this;
        }

        /// <summary>
        /// Adding a body to request with default request format as <see cref="DataFormat"/>
        /// </summary>
        /// <param name="body">string body</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithBody(string body)
        {
            var contentType = _request.XmlSerializer.ContentType;
            if (_baseDataFormat == DataFormat.Json)
            {
                contentType = _request.JsonSerializer.ContentType;
            }

            _request.AddParameter(contentType, body, ParameterType.RequestBody);
            _requestInformation.AppendLine($"Body: {body}");
            return this;
        }

        /// <summary>
        /// Adding a body to request
        /// </summary>
        /// <param name="body">string body</param>
        /// <param name="requestFormat">Request format</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithBody(string body, DataFormat requestFormat)
        {
            WithRequestFormat(requestFormat);
            return WithBody(body);
        }

        /// <summary>
        /// Adding parameter to the request
        /// </summary>
        /// <param name="name">parameter name</param>
        /// <param name="value">parameter value</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithParameter(string name, object value)
        {
            _request.AddParameter(name, value);
            _requestInformation.AppendLine($"Parameter: {name} with value {value}");
            return this;
        }

        public CustomRestRequest WithQueryParameter(string name, string value)
        {
            _request.AddQueryParameter(name, value);
            _requestInformation.AppendLine($"Parameter: {name} with value {value}");
            return this;
        }

        /// <summary>
        /// Adding parameter to the request
        /// </summary>
        /// <param name="name">parameter name</param>
        /// <param name="value">parameter value</param>
        /// <param name="type">parameter type</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithParameter(string name, object value, ParameterType type)
        {
            _request.AddParameter(name, value, type);
            _requestInformation.AppendLine($"Parameter: {name} with value {value} with type {type}");
            return this;
        }

        /// <summary>
        /// Adding parameters to the request
        /// </summary>
        /// <param name="names">Names of parameters.</param>
        /// <param name="values">Values of parameters.</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithParameters(string[] names, object[] values)
        {
            if (names == null)
            {
                return this;
            }

            foreach (var name in names)
            {
                var index = names.ToList().IndexOf(name);
                if (values != null)
                {
                    WithParameter(name, values[index]);
                }
            }

            return this;
        }

        /// <summary>
        /// Adding Url segment
        /// </summary>
        /// <param name="name">Url segment name</param>
        /// <param name="value">Url segment value</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithUrlSegment(string name, string value)
        {
            _request.AddUrlSegment(name, value);
            _requestInformation.AppendLine($"Url segment: {name} with value {value}");
            return this;
        }

        /// <summary>
        /// Adding a header to request
        /// </summary>
        /// <param name="name">header name</param>
        /// <param name="value">header value</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithHeader(string name, string value)
        {
            _request.AddHeader(name, value);
            _requestInformation.AppendLine($"Header: {name} with value {value}");
            return this;
        }

        /// <summary>
        /// Adding request format as <see cref="DataFormat"/>
        /// </summary>
        /// <param name="dataFormat">request format as <see cref="DataFormat"/></param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithRequestFormat(DataFormat dataFormat)
        {
            _baseDataFormat = dataFormat;
            _request.RequestFormat = dataFormat;
            _requestInformation.AppendLine($"Request format: {dataFormat}");
            return this;
        }

        /// <summary>
        /// Adds the bytes to the Files collection with the specified file name
        /// </summary>
        /// <param name="name">parameter name</param>
        /// <param name="fileStream">file stream</param>
        /// <param name="fileName">file name</param>
        /// <param name="contentType">content type</param>
        /// <returns>Request as <see cref="CustomRestRequest"/></returns>
        public CustomRestRequest WithFile(string name, byte[] fileStream, string fileName, string contentType)
        {
            _request.AddFile(name, fileStream, fileName, contentType);
            _requestInformation.AppendLine($"name: {name}; file name: {fileName}; content type: {contentType}");
            return this;
        }

        /// <summary>
        /// Rest request builder
        /// </summary>
        /// <returns>request as <see cref="RestClient"/></returns>
        public RestRequest Build()
        {
            return _request;
        }

        /// <summary>
        /// Detailed information
        /// </summary>
        /// <returns>formatted request</returns>
        public override string ToString()
        {
            return $"Detailed information:\r\n {_requestInformation}";
        }
    }
}
