
namespace Tests.Helpers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;

    using Core.Service.JsonProcess;

    using global::Tests.Managers;

    using RestSharp;

    public abstract class ApiHelperTemplate : HelperTemplate
    {
        protected ApiHelperTemplate(ApplicationManager app)
            : base(app)
        {
        }

        private Dictionary<string, IRestClient> clients = new Dictionary<string, IRestClient>();

        public IRestResponse Execute(IRestClient client, RestRequest request)
        {
            return this.ExecuteRequest(client, request, 4, true);
        }

        public IRestResponse Execute(IRestClient client, RestRequest request, int repeatTimes)
        {
            return this.ExecuteRequest(client, request, repeatTimes, true);
        }

        public IRestResponse Execute(IRestClient client, RestRequest request, bool valid)
        {
            return this.ExecuteRequest(client, request, 4, valid);
        }

        public IRestResponse Execute(IRestClient client, RestRequest request, int repeatTimes, bool valid)
        {
            return this.ExecuteRequest(client, request, repeatTimes, valid);
        }

        public IRestResponse<List<T>> Execute<T>(IRestClient client, RestRequest request) where T : new()
        {
            return this.ExecuteRequest<T>(client, request, 4, true);
        }

        public IRestResponse<List<T>> Execute<T>(IRestClient client, RestRequest request, int repeatTimes) where T : new()
        {
            return this.ExecuteRequest<T>(client, request, repeatTimes, true);
        }

        public IRestResponse<List<T>> Execute<T>(IRestClient client, RestRequest request, bool valid) where T : new()
        {
            return this.ExecuteRequest<T>(client, request, 4, valid);
        }

        public IRestResponse<List<T>> Execute<T>(IRestClient client, RestRequest request, int repeatTimes, bool valid) where T : new()
        {
            return this.ExecuteRequest<T>(client, request, repeatTimes, valid);
        }

        protected IRestClient CreateClient(string baseUrl = null, int timeoutMsec = 120000)
        {
            baseUrl = string.IsNullOrEmpty(baseUrl) ? Parameters.Parameters.ToolManagerUrl : baseUrl;
            RestClient restClient = new RestClient(baseUrl);
            restClient.CookieContainer = new CookieContainer();
            restClient.AddHandler("application/json", NewtonsoftCustom.Default);
            restClient.AddHandler("text/json", NewtonsoftCustom.Default);
            restClient.AddHandler("*+json", NewtonsoftCustom.Default);
            restClient.Timeout = timeoutMsec;
            return restClient;
        }

        private IRestResponse ExecuteRequest(IRestClient client, RestRequest request, int repeatTimes = 4, bool validRequest = true)
        {
            int num = 1;
            bool flag = false;
            IRestResponse response = null;
            for (; !flag && num <= repeatTimes; ++num)
            {
                response = client.Execute(request);
                int statusCode = (int)response.StatusCode;
                if (statusCode == 0)
                {
                    if (num == repeatTimes)
                        this.ExceptionProcessing(request, response);
                    Thread.Sleep(2000);
                }
                else if ((statusCode < 200 || statusCode > 399) && validRequest)
                {
                    if (num == repeatTimes)
                        this.ExceptionProcessing(request, response);
                    Thread.Sleep(2000);
                }
                else
                    flag = true;
            }

            return response;
        }

        private IRestResponse<List<T>> ExecuteRequest<T>(IRestClient client, RestRequest request, int repeatTimes = 4, bool validRequest = true) where T : new()
        {
            int num = 1;
            bool flag = false;
            IRestResponse<List<T>> restResponse = null;
            for (; !flag && num <= repeatTimes; ++num)
            {
                restResponse = client.Execute<List<T>>(request);
                int statusCode = (int)restResponse.StatusCode;
                if (statusCode == 0)
                {
                    if (num == repeatTimes)
                        this.ExceptionProcessing(request, restResponse);
                    Thread.Sleep(1000);
                }
                else if ((statusCode < 200 || statusCode > 399) && validRequest)
                {
                    if (num == repeatTimes)
                        this.ExceptionProcessing(request, restResponse);
                    Thread.Sleep(1000);
                }
                else
                    flag = true;
            }

            return restResponse;
        }
        
        private void ExceptionProcessing(RestRequest request, IRestResponse response)
        {
            App.Logger.Info("Request was sent to : " + request.Resource);
            App.Logger.Error("Erroneous response : " + response.Content);
            throw new Exception("Erroneous response status code: " + response.StatusCode);
        }

        public IRestClient GetClient(string clientName)
        {
            IRestClient restClient;
            this.clients.TryGetValue(clientName, out restClient);
            return restClient;
        }

        public void SetClient(string clientName, IRestClient client)
        {
            IRestClient restClient;
            if (this.clients.TryGetValue(clientName, out restClient))
                this.clients[clientName] = client;
            else
                this.clients.Add(clientName, client);
        }
    }
}
