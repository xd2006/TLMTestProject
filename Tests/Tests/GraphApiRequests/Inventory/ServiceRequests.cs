
namespace Tests.GraphApiRequests.Inventory
{
    using GraphQL.Common.Request;

    public class ServiceRequests
    {
        public GraphQLRequest GetCutterGroups()
        {
            var request = new GraphQLRequest
            {
                Query = @"
              {
                cutterGroups
                    {
                        name
                        parentId 
                        id
                    }
              }",
                OperationName = null,
                Variables = null
                              };
            return request;
        }

        public GraphQLRequest GetCuttingMaterials()
        {
            var request = new GraphQLRequest
                              {
                                  Query = @"
              {
                cuttingMaterials
                    {
                        name
                        id
                    }
              }",
                                  OperationName = null,
                                  Variables = null
                              };
            return request;
        }

        public GraphQLRequest GetUsageMaterials()
        {
            var request = new GraphQLRequest
                              {
                                  Query = @"
              {
                machinedMaterials
                    {
                        name
                        id
                    }
              }",
                                  OperationName = null,
                                  Variables = null
                              };
            return request;
        }
    }
}
