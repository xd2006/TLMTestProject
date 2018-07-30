
namespace Tests.Helpers.GraphApi.ProjectManager
{
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.GraphQlModels;
    using global::Tests.TestsData.Orders;

    using GraphQL.Client;
    using GraphQL.Common.Request;

    using NUnit.Framework;

    public class GraphApiProjectManagerHelper : GraphApiHelperTemplate
    {

        public GraphApiProjectManagerHelper(ApplicationManager app)
            : base(app)
        {
        }

        new protected GraphQLClient Client => base.Client("ProjectManager");

        public int CreateOrder(Order order)
        {
            var request = ProjectManagerRequests.Order.CreateOrder(order);
            var result = ExecuteRequestWithRetry(Client, request, 3);

            return result.Data["createOrder"]["id"];
        }

        public List<Order> GetOrders(int take = 1000)
        {
            GetEntitiesRequest request = ProjectManagerRequests.Order.GetOrders;

            return ExecuteRequestByPages<Order>(take, request, "orders", 25);

        }

        public int CreateWorkpiece(Workpiece workpiece)
        {
            var request = ProjectManagerRequests.Order.CreateWorkpiece(workpiece);
            var result = this.Client.PostAsync(request).Result;
            return result.Data["createWorkpiece"]["id"];
        }

        public int CreateWorkPlan(Workplan workplan)
        {
            var request = ProjectManagerRequests.Order.CreateWorkplan(workplan);
            var result = this.Client.PostAsync(request).Result;
            return result.Data["createWorkplan"]["id"];

        }

        public void AddOrdersIfNeeded(int neededNumberOfOrders)
        {
            int ordersCount = GetOrders().Count;
            if (ordersCount < neededNumberOfOrders)
            {
                for (int i = 1; i <= neededNumberOfOrders - ordersCount; i++)
                    CreateOrderWithWorkpieces(4);
            }
        }

        public int CreateTask(Task task)
        {
            var request = ProjectManagerRequests.Order.CreateTask(task);
            var result = this.Client.PostAsync(request).Result;
            return result.Data["createTask"]["id"];
        }


        public void CreateOrderWithWorkpieces(int maxNumberOfWorkpieces)
        {
            var testOrder = new Faker<Order>()
                .RuleFor(
                    o => o.ExternalOrderId,
                    f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000))
                .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
                .RuleFor(o => o.Comments, f => f.Hacker.Phrase()).RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));

            Order orderToCreate = testOrder;

            var orderId = this.App.GraphApi.ProjectManager.CreateOrder(orderToCreate);

            var testWorkpiece = new Faker<Workpiece>().Rules(
                (f, o) =>
                    {
                        o.Name = f.Hacker.Noun();
                        o.DeliveryDate = f.Date.Future(1);
                        o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation();
                        o.OrderId = orderId;
                        o.Quantity = f.Random.Int(2, 10000);
                    });

            var faker = new Faker("en");
            var numberOfWorkPieces = faker.Random.Int(1, maxNumberOfWorkpieces);

            for (int i = 1; i <= numberOfWorkPieces; i++)
            {
                var workpiece = testWorkpiece;
                var wpId = this.App.GraphApi.ProjectManager.CreateWorkpiece(workpiece);

                Workplan workplan = new Workplan();
                workplan.WorkpieceId = wpId;

                this.App.GraphApi.ProjectManager.CreateWorkPlan(workplan);
            }
        }

        public Machine GetMachine(int machineId)
        {
            var machines = this.GetMachines();
            return machines.First(m => m.id.Equals(machineId));
        }

        public List<Machine> GetMachines()
        {
            var request = ProjectManagerRequests.Order.GetMachines();
            var machines = ExecuteRequestWithRetry(Client, request).GetDataFieldAs<List<Machine>>("machines");

            return machines;
        }

        private List<T> ExecuteRequestByPages<T>(
            int take,
            GetEntitiesRequest request,
            string targetSection,
            int pageSize = 25)
        {
            return ExecuteRequestByPages<T>(Client, take, request, targetSection, pageSize);
        }

        public Workpiece GetWorkpiece(int workpieceId)
        {
            var request = ProjectManagerRequests.Order.GetWorkpiece(workpieceId);
            var response = ExecuteRequestWithRetry(Client, request);
            var workpiece = response.GetDataFieldAs<Workpiece>("workpiece");
            return workpiece;
        }

        public List<Task> GetMachineTasks(int machineId)
        {
            var request = ProjectManagerRequests.Order.GetMachineTasks(machineId.ToString());
            var response = ExecuteRequestWithRetry(Client, request);
            var tasks = response.GetDataFieldAs<List<Task>>("machineTasks");
            return tasks;
        }
    }
}
