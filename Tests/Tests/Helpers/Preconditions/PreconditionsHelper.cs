
namespace Tests.Helpers.Preconditions
{
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    public class PreconditionsHelper : HelperTemplate
    {
        public PreconditionsHelper(ApplicationManager app)
            : base(app)
        {
        }

        public Dictionary<GeneralData.ProjectManagerEnitityTypes, int> CreateOrderWorkpieceWorkplan()
        {
            Dictionary<GeneralData.ProjectManagerEnitityTypes, int> ids = new Dictionary<GeneralData.ProjectManagerEnitityTypes, int>();

            var orderId = this.CreateOrder();

            ids[GeneralData.ProjectManagerEnitityTypes.Order] = orderId;

            var workpieceId = this.CreateWorkpiece(orderId);

            ids[GeneralData.ProjectManagerEnitityTypes.Workpiece] = workpieceId;

            var workplanId = this.CreateWorkplan(workpieceId);

            ids[GeneralData.ProjectManagerEnitityTypes.Workplan] = workplanId;

            return ids;
        }

        public int CreateWorkplan(int workpieceId)
        {
            Workplan workplan = new Workplan();
            workplan.WorkpieceId = workpieceId;

            var workplanId = this.App.GraphApi.ProjectManager.CreateWorkPlan(workplan);
            return workplanId;
        }

        public int CreateWorkpiece(int orderId)
        {
            var testWorkpiece = new Faker<Workpiece>().Rules(
                (f, o) =>
                    {
                        o.Name = f.Hacker.Noun() + "_" + f.Hacker.Adjective();
                        o.DeliveryDate = f.Date.Future(1);
                        o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation() + "_" + f.Finance.Amount();
                        o.OrderId = orderId;
                        o.Quantity = f.Random.Int(2, 10000);
                    });

            Workpiece workpiece = testWorkpiece;
            var workpieceId = this.App.GraphApi.ProjectManager.CreateWorkpiece(workpiece);
            return workpieceId;
        }

        public int CreateWorkpiece(Workpiece workpiece)
        {
            var workpieceId = this.App.GraphApi.ProjectManager.CreateWorkpiece(workpiece);
            return workpieceId;
        }

        public int CreateOrder(Order orderToCreate = null)
        {
            var testOrder = new Faker<Order>()
                .RuleFor(o => o.ExternalOrderId, f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000))
                .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
                .RuleFor(o => o.Comments, f => f.Hacker.Phrase()).RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));

            orderToCreate = orderToCreate ?? testOrder;

            var orderId = this.App.GraphApi.ProjectManager.CreateOrder(orderToCreate);
            return orderId;
        }

        public int CreateTask(int workplanId)
        {
            var taskGenerationRule = new Faker<Task>().Rules(
                (f, o) =>
                    {
                        o.Name = "Task_" + f.Hacker.Noun() + "_" + f.Random.Int(1, 3000);
                        o.DurationPerWorkpiece = f.Random.Int(1, 3000);
                        o.DurationPerTotal = o.DurationPerWorkpiece * f.Random.Int(2, 100);
                        o.StartDate = f.Date.Future(2);
                        o.EndDate = o.StartDate.AddDays(f.Random.Int(1, 100));
                        o.MachineId = f.PickRandom(App.GraphApi.ProjectManager.GetMachines().Select(m => m.id));
                        o.WorkplanId = workplanId;
                    });

            var task = taskGenerationRule.Generate();
            return App.GraphApi.ProjectManager.CreateTask(task);
        }

        public int CreateTask(Task task)
        {
            return App.GraphApi.ProjectManager.CreateTask(task);
        }
    }
}
