
namespace Tests.Helpers.GraphApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using global::Tests.GraphApiRequests.RequestsManagers;

    using GraphQL.Client;

    using global::Tests.Managers;
    using global::Tests.Parameters;

    using GraphQL.Common.Request;
    using GraphQL.Common.Response;

    using NUnit.Framework.Constraints;

    public abstract class GraphApiHelperTemplate : HelperTemplate
    {
        protected delegate GraphQLRequest GetEntitiesRequest(
            int take,
            int skip);

        private readonly Dictionary<string, GraphQLClient> clients = new Dictionary<string, GraphQLClient>();

        private ToolManagerRequestsManager toolManagerRequests;

        private ProjectManagerRequestManager projectManagerRequests;

        protected GraphApiHelperTemplate(ApplicationManager app)
            : base(app)
        {
        }

        protected ToolManagerRequestsManager ToolManagerRequests =>
            this.toolManagerRequests ?? (this.toolManagerRequests = new ToolManagerRequestsManager());

        protected ProjectManagerRequestManager ProjectManagerRequests =>
            this.projectManagerRequests ?? (this.projectManagerRequests = new ProjectManagerRequestManager());

        protected GraphQLClient Client(string app = "ToolManager")
        {
            GraphQLClient client = null;
            switch (app)
            {
                case "ToolManager":
                    {
                        if (this.TryGetClient(app))
                            {
                                client = this.clients[app];
                            }
                            else
                            {
                                client = new GraphQLClient(Parameters.ToolManagerGraphQlEndpoint);

                                this.clients.Add(app, client);
                            }                       

                        break;
                    }
                case "ProjectManager":
                    {
                        if (this.TryGetClient(app))
                        {
                            client = this.clients[app];
                        }
                        else
                        {
                            client = new GraphQLClient(global::Tests.Parameters.Parameters.ProjectManagerGraphQlEndpoint);

                            this.clients.Add(app, client);
                        }

                        break;
                    }
            }

            return client;
        }


        protected List<T> ExecuteRequestByPages<T>(GraphQLClient client, int take, GetEntitiesRequest request, string targetSection, int pageSize = 25)
        {
            List<T> entities = new List<T>();

            int skipped = 0;
            bool moresResultsExist = true;
            while (moresResultsExist && pageSize > 0)
            {
                pageSize = skipped + pageSize > take ? take - skipped : pageSize;

                if (pageSize > 0)
                {
                    var result = client.PostAsync(
                        request(pageSize, skipped));
                    var resultsToAdd = result.Result.GetDataFieldAs<List<T>>(targetSection);
                    if (resultsToAdd.Count > 0)
                    {
                        entities.AddRange(resultsToAdd);
                    }

                    moresResultsExist = resultsToAdd.Count == pageSize;

                    skipped += pageSize;
                }
            }

            return entities;
        }


        protected GraphQLResponse ExecuteRequestWithRetry(GraphQLClient client, GraphQLRequest request, int retries = 10)
        {
            int i = 0;
            GraphQLResponse response = null;
            Exception ex = null;
            string message = string.Empty;

            do
            {
                message = string.Empty;
                try
                {
                    response = client.PostAsync(request).Result;
                }
                catch (Exception e)
                {
                    message = e.Message;
                    Thread.Sleep(500);
                    ex = e;
                }
            }
            while (message != string.Empty && i++ < retries);

            if (i >= 10)
            {
                throw ex;
            }

            return response;
        }


        private bool TryGetClient(string app)
        {
            if (this.clients != null)
            {
                return this.clients.ContainsKey(app);
            }
            return false;
        }
    }   
}
