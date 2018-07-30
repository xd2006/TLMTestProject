
namespace Tests.GraphApiRequests.Orders
{
    using System.Collections.Generic;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;

    using GraphQL.Common.Request;

    public class OrderRequests
    {
        public GraphQLRequest CreateOrder(Order order)
       {
            Dictionary<string, object> data = new Dictionary<string, object>
                                                  {
                                                      { "id", null },
                                                      { "externalOrderId", order.ExternalOrderId },
                                                      { "customerId", order.CustomerId },
                                                      {
                                                          "deliveryDate",
                                                          order.DeliveryDate.ToString("yyyy-MM-dd")
                                                      },
                                                      { "comments", order.Comments }                                                                                                                                                                                                                                                                     
                                                  };


            var request = new GraphQLRequest
                              {
                OperationName = null,
                Variables = new
                {
                    _v0_order = data
                                },
                                  Query = @"mutation ($_v0_order: OrderInputType!) {createOrder(order: $_v0_order) {
                id
                __typename
    		    }
                  }"
            };

            return request;
        }


        public GraphQLRequest GetOrderWorkpieces(int orderId)
        {
            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Query = @"
               query ($_v0_id: Int!)	                                                  
               {
                order(id: $_v0_id) {
               
            workpieces
            {
            id
            externalWorkpieceId
            name
            quantity
            deliveryDate
            __typename
                }
                  }",
                                  Variables = new
                                                  {
                                                      _v0_id = orderId,
                                                  }
                              };
            return request;


        }





        public GraphQLRequest GetOrders(int take, int skip)
        {
            Dictionary<string, string> paging = new Dictionary<string, string>
                                                    {
                                                        { "take", take.ToString() },
                                                        { "skip", skip.ToString() }
                                                    };


            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Query = @"
               query ($_v0_paging: PagingInputType)	                                                  
               {
                orders(paging: $_v0_paging) {
               
            id
            externalOrderId
            customerId
            deliveryDate
            createdDate
            createdBy
            comments
            __typename
                }
                  }",
                                Variables = new
                                                  {
                                                      _v0_paging = paging,
                                                  }
            };
            return request;
        }

        public GraphQLRequest CreateWorkpiece(Workpiece workpiece)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
                                                  {
                                                      { "id", null },
                                                      { "externalWorkpieceId", workpiece.ExternalWorkpieceId },
                                                      { "name", workpiece.Name },
                                                      {
                                                          "orderId",
                                                          workpiece.OrderId
                                                      },
                                                      { "deliveryDate", workpiece.DeliveryDate.ToString("yyyy-MM-dd") },
                                                      { "quantity", workpiece.Quantity }
                                                  };


            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_workpiece = data
                                                  },
                                  Query = @"mutation ($_v0_workpiece: WorkpieceInputType!) {createWorkpiece(workpiece: $_v0_workpiece) {
                id
                __typename
    		    }
                  }"
            };

            return request;
        }

        public GraphQLRequest CreateWorkplan(Workplan workplan)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
                                                  {
                                                      { "workpieceId", workplan.WorkpieceId },
                                                  };

            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_workplan = data
                                                  },
                                  Query = @"mutation ($_v0_workplan: WorkplanInputType!) {createWorkplan(workplan: $_v0_workplan) {
                id
                __typename
    		    }
                  }"
                              };

            return request;
        }

        public GraphQLRequest CreateTask(Task task)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
                                                  {
                                                      { "durationPerTotal", task.DurationPerTotal },
                                                      { "durationPerWorkpiece", task.DurationPerWorkpiece },
                                                      { "endDate", task.EndDate.ToString("yyyy-MM-dd") },
                                                      { "id", null },
                                                      { "machineId", task.MachineId },
                                                      { "name", task.Name },
                                                      { "startDate", task.StartDate.ToString("yyyy-MM-dd") },
                                                      { "workplanId", task.WorkplanId },
                                                  };

            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_task = data
                                                  },
                                  Query = @"mutation ($_v0_task: TaskInputType!) {createTask(task: $_v0_task) {
                id
                __typename
    		    }
                  }"
                              };

            return request;
        }

        public GraphQLRequest GetMachines()
        {
           
            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = null,
                                  Query = @"query {machines {
                id
                name
                __typename
    		    }
                  }"
            };

            return request;
        }

        public GraphQLRequest GetMachineTasks(string machineId)
        {
            var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_id = machineId
                                                  },

                                  Query = @"query ($_v0_id: String!) {machineTasks(machineId: $_v0_id ) {
                id
                name
               durationPerWorkpiece
  			  durationPerTotal
              machineId
  			  startDate
              endDate
              workplan {
                id
              }

    		    }
                  }"
            };

            return request;
        }

        public GraphQLRequest GetWorkpiece(int workpieceId)
        {
           var request = new GraphQLRequest
                              {
                                  OperationName = null,
                                  Variables = new
                                                  {
                                                      _v0_id = workpieceId
                                                  },
                                  Query = @"query ($_v0_id: Int!) {workpiece(id: $_v0_id) {
                orderId
                externalWorkpieceId
                name
                quantity
                deliveryDate
    		    }
                  }"
                              };

            return request;
        }
    }
}
