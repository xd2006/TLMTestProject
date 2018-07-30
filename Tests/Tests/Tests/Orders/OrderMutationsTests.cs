
namespace Tests.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using Core.Service;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("ProjectManagerAPIMutations")]
    public class OrderMutationsTests : TestBase
    {
        [Test]
        public void CheckOrderWorkpieceWorkplanMutations()
        {
            var testOrder = new Faker<Order>()
                .RuleFor(o => o.ExternalOrderId, f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000))
                .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
                .RuleFor(o => o.Comments, f => f.Hacker.Phrase()).RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));

            Order orderToCreate = testOrder;

            var orderId = this.App.GraphApi.ProjectManager.CreateOrder(orderToCreate);
            
            var orderDb = this.App.Db.ProjectManager.GetOrder(orderId);

            Assert.That(
                orderToCreate.ExternalOrderId.Equals(orderDb.ExternalOrderId)
                && orderToCreate.CustomerId.Equals(orderDb.CustomerId)
                && orderToCreate.Comments.Equals(orderDb.Comments) && orderDb.CreatedDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")),
                "Order wasn't created correctly. Some data is wrong");

            var orders = this.App.Db.ProjectManager.GetOrders().Where(o => o.ExternalOrderId.Equals(orderToCreate.ExternalOrderId))
                .ToList();

            Assert.That(orders.Count == 1, $"Only 1 order should have been created. But was {orders.Count}");

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

            var workpieceDb = this.App.Db.ProjectManager.GetWorkpiece(workpieceId);

            bool workpieceIsCorrect = workpiece.Name.Equals(workpieceDb.Name)
                                       && workpiece.DeliveryDate.ToString("yyyy-MM-dd").Equals(
                                           workpieceDb.DeliveryDate.ToString("yyyy-MM-dd"))
                                       && workpiece.ExternalWorkpieceId.Equals(workpieceDb.ExternalWorkpieceId)
                                       && workpiece.OrderId.Equals(workpieceDb.OrderId)
                                       && workpiece.Quantity.Equals(workpieceDb.Quantity) && workpieceDb.CreatedDate
                                           .ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd"));
            string errorMessage = string.Empty;

            if (!workpieceIsCorrect)
            {
                errorMessage = this.FormWorkpieceErrorMessage(workpiece, workpieceDb);
            }

            Assert.That(
                workpieceIsCorrect,
                $"Workpiece wasn't created correctly. Some data is wrong. Workpiece Id -  {workpiece.ExternalWorkpieceId}. {errorMessage}");

            var workpieces = this.App.Db.ProjectManager.GetWorkpieces()
                .Where(w => w.ExternalWorkpieceId.Equals(workpiece.ExternalWorkpieceId)).ToList();

            Assert.That(workpieces.Count == 1, $"Only 1 workpiece should have been created. But was {workpieces.Count}. ExternalWorkpieceId is '{workpiece.ExternalWorkpieceId}'");

            Workplan workplan = new Workplan();
                workplan.WorkpieceId = workpieceId;

               var workplanId = this.App.GraphApi.ProjectManager.CreateWorkPlan(workplan);

            var workplanDb = this.App.Db.ProjectManager.GetWorkplan(workplanId);

            Assert.That(
                workplan.WorkpieceId.Equals(workplanDb.WorkpieceId) && workplanDb.CreatedDate.ToString("yyyy-MM-dd")
                    .Equals(DateTime.Now.ToString("yyyy-MM-dd")),
                "Workplan wasn't created correctly. Some data is wrong");

            var workplans = this.App.Db.ProjectManager.GetWorkplans().Where(w => w.WorkpieceId.Equals(workpieceId))
                .ToList();

            Assert.That(workplans.Count == 1, $"Only 1 workplan should have been created. But was {workplans.Count}");
        }

        [Test]
        public void CheckTaskMutation()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            var taskGenerationRule = new Faker<Task>().Rules(
                (f, o) =>
                    {
                        o.Name = "Task_" + f.Hacker.Noun() + "_" + f.Random.Int(1, 3000);
                        o.DurationPerWorkpiece = f.Random.Int(1, 3000);
                        o.DurationPerTotal = o.DurationPerWorkpiece * f.Random.Int(2, 100);
                        o.StartDate = f.Date.Future(2);
                        o.EndDate = o.StartDate.AddDays(f.Random.Int(1, 100));
                        o.MachineId = f.PickRandom(this.App.GraphApi.ProjectManager.GetMachines().Select(m => m.id));
                        o.WorkplanId = ids[GeneralData.ProjectManagerEnitityTypes.Workplan];
                    });

            var task = taskGenerationRule.Generate();
            
            var taskId = this.App.GraphApi.ProjectManager.CreateTask(task);

            var taskDb = this.App.Db.ProjectManager.GetTask(taskId);

            bool taskIsCorrect = task.Name.Equals(taskDb.Name)
                                 && task.StartDate.ToString("yyyy-MM-dd")
                                     .Equals(taskDb.StartDate.ToString("yyyy-MM-dd"))
                                 && task.EndDate.ToString("yyyy-MM-dd").Equals(taskDb.EndDate.ToString("yyyy-MM-dd"))
                                 && task.DurationPerWorkpiece.Equals(taskDb.DurationPerWorkpiece)
                                 && task.MachineId.Equals(taskDb.MachineId) && task.WorkplanId.Equals(taskDb.WorkplanId)
                                 && taskDb.CreatedDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd"));
            Assert.True(taskIsCorrect, "Task wasn't created correctly in DB");
        }

        private string FormWorkpieceErrorMessage(Workpiece workpiece, Workpiece workpieceDb)
        {
            List<string> errorMessage = new List<string>();
            
                if (!workpiece.Name.Equals(workpieceDb.Name))
                {
                    errorMessage.Add($"WorkpieceName: expected '{workpiece.Name}', actual in DB '{workpieceDb.Name}'");
                }

                if (!workpiece.DeliveryDate.ToString("yyyy-MM-dd").Equals(workpieceDb.DeliveryDate.ToString("yyyy-MM-dd")))
                {
                    errorMessage.Add(
                        $"Workpiece Delivery Date: expected '{workpiece.DeliveryDate:yyyy-MM-dd}', actual in DB '{workpieceDb.DeliveryDate:yyyy-MM-dd}'");
                }

                if (!workpiece.ExternalWorkpieceId.Equals(workpieceDb.ExternalWorkpieceId))
                {
                    errorMessage.Add(
                        $"External Workpiece Id: expected '{workpiece.ExternalWorkpieceId}', actual in DB '{workpieceDb.ExternalWorkpieceId}'");
                }

                if (!workpiece.OrderId.Equals(workpieceDb.OrderId))
                {
                    errorMessage.Add($"Order Id: expected '{workpiece.OrderId}', actual in DB '{workpieceDb.OrderId}'");
                }

                if (!workpiece.Quantity.Equals(workpieceDb.Quantity))
                {
                    errorMessage.Add(
                        $"Workpiece Quantity: expected '{workpiece.Quantity}', actual in DB '{workpieceDb.Quantity}'");
                }

                if (!workpieceDb.CreatedDate.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    errorMessage.Add(
                        $"Workpiece CreatedDate: expected '{workpiece.Quantity}', actual in DB '{workpieceDb.CreatedDate:yyyy-MM-dd}");
                }
            
            return ServiceMethods.ListToString(errorMessage);
        }
    }
}
