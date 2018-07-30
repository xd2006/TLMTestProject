
namespace Tests.GraphApiRequests.Inventory
{
    using GraphQL.Common.Request;

    public class DetailsRequests
    {
        public GraphQLRequest GetToolAssemblyDetails(string toolAssemblyId)
        {
            var request = new GraphQLRequest
                              {
                                  Query =
                                      @"query ($_v0_toolAssemblyId: String!) { 
toolAssembly(toolAssemblyId: $_v0_toolAssemblyId) {
    id
    name
    length
    description
    quantity
    usageMaterials
    cutterAssembly {
        cutter {
             id
             name
             diameter
             edgeNr
             __typename
              }
            cuttingMaterial {
            type
            __typename
              }
exchangablePlate {
             id
             name
             __typename
             }
             __typename
             }
            holders {
             id
             name
             __typename
                   }
                  __typename
             }
}",
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_toolAssemblyId = toolAssemblyId,
                                                  }                             
                              };

            return request;
        }
    }
}
