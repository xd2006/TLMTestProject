
namespace Tests.Tests.Orders.Templates
{
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using Core.Service;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders;

    using NUnit.Framework;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("ProjectManagerGrid")]
    public abstract class OrdersTestTemplate : TestBase
    {
        [SetUp]
        public void BeforeEachTest()
        {
            this.App.BaseUrl = Parameters.Parameters.ProjectManagerUrl;
        }

        #region Generation rules
        protected Faker<Order> OrderGenerationRule => new Faker<Order>()
            .RuleFor(o => o.ExternalOrderId, f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000))
            .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
            .RuleFor(o => o.Comments, f => f.Hacker.Phrase())
            .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));


        protected Faker<Workpiece> WorkpieceGenerationRule => new Faker<Workpiece>().Rules(
            (f, o) =>
                {
                    o.Name = f.Hacker.Noun();
                    o.DeliveryDate = f.Date.Future(1);
                    o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation();
                    o.Quantity = f.Random.Int(2, 10000);
                });

        protected Faker<Task> TaskGenerationRule => new Faker<Task>().Rules(
            (f, o) =>
                {
                    o.Name = "Task_" + f.Hacker.Noun() + "_" + f.Random.Int(1, 3000);
                    o.DurationPerWorkpiece = f.Random.Int(1, 3000);
                    o.DurationPerTotal = o.DurationPerWorkpiece * f.Random.Int(2, 100);
                    o.StartDate = f.Date.Future(2);
                    o.EndDate = o.StartDate.AddDays(f.Random.Int(1, 100));
                    o.MachineId = f.PickRandom(this.App.GraphApi.ProjectManager.GetMachines().Select(m => m.id));
                });

        #endregion

        #region Helper methods
        protected KeyValuePair<bool, string> CompareOrderWithOrderFromGrid(Order order, Order orderExpected)
        {
            if (order != null && orderExpected != null)
            {
                string errorMessage = string.Empty;
                List<string> errorLines = new List<string>();
                bool valid = order.ExternalOrderId.Equals(orderExpected.ExternalOrderId)
                       && order.CustomerId.Equals(orderExpected.CustomerId)
                       && order.DeliveryDate.ToString("yyyy-MMMM-dd")
                                 .Equals(orderExpected.DeliveryDate.ToString("yyyy-MMMM-dd"));

                if (!valid)
                {
                    if (!order.ExternalOrderId.Equals(orderExpected.ExternalOrderId))
                    {
                        errorLines.Add($"External Order Id: expected '{orderExpected.ExternalOrderId}', actual '{order.ExternalOrderId}'");
                    }

                    if (!order.CustomerId.Equals(orderExpected.CustomerId))
                    {
                        errorLines.Add($"External Order Id: expected '{orderExpected.CustomerId}', actual '{order.CustomerId}'");
                    }

                    if (!order.DeliveryDate.ToString("yyyy-MMMM-dd")
                            .Equals(orderExpected.DeliveryDate.ToString("yyyy-MMMM-dd")))
                    {
                        errorLines.Add($"Order Delivery Date: expected '{orderExpected.DeliveryDate:yyyy-MMMM-dd}', actual '{order.DeliveryDate:yyyy-MMMM-dd}'");
                    }
                }

                if (errorLines.Count > 0)
                {
                    errorMessage = ServiceMethods.ListToString(errorLines);
                }
                return new KeyValuePair<bool, string>(valid, errorMessage);
            }

            return new KeyValuePair<bool, string>(false, "One of the orders is null");
        }

        protected KeyValuePair<bool, string> CompareWorkpieceWithWorkpieceFromGrid(WorkpieceGridRecord workpieceToCheck, Workpiece wpExpected)
        {
            if (workpieceToCheck != null && wpExpected != null)
            {

                string errorMessage = string.Empty;
                List<string> errorLines = new List<string>();
                bool valid = workpieceToCheck.Id.Equals(wpExpected.ExternalWorkpieceId)
                             && workpieceToCheck.Name.Equals(wpExpected.Name)
                             && workpieceToCheck.Quantity.Equals(wpExpected.Quantity.ToString());

                if (!Parameters.Parameters.Browser.Equals("MicrosoftEdge"))
                {
                    valid = valid && workpieceToCheck.DeliveryDate.Equals(
                        wpExpected.DeliveryDate.ToString("MM/dd/yyyy"));
                }

                if (!valid)
                {
                    if (!workpieceToCheck.Id.Equals(wpExpected.ExternalWorkpieceId))
                    {
                        errorLines.Add($"Workpiece Id: expected '{wpExpected.ExternalWorkpieceId}', actual '{workpieceToCheck.Id}'");
                    }

                    if (!workpieceToCheck.DeliveryDate.Equals(wpExpected.DeliveryDate.ToString("MM/dd/yyyy")))
                    {
                        errorLines.Add($"Workpiece Delivery date: expected '{ wpExpected.DeliveryDate.ToString("MM/dd/yyyy")}', actual '{workpieceToCheck.DeliveryDate}'");
                    }

                    if (!workpieceToCheck.Name.Equals(wpExpected.Name))
                    {
                        errorLines.Add($"Name: expected '{wpExpected.Name}', actual '{workpieceToCheck.Name}'");
                    }

                    if (!workpieceToCheck.Quantity.Equals(wpExpected.Quantity.ToString()))
                    {
                        errorLines.Add($"Quantity: expected '{wpExpected.Quantity}', actual '{workpieceToCheck.Quantity}'");
                    }
                }

                if (errorLines.Count > 0)
                {
                    errorMessage = ServiceMethods.ListToString(errorLines);
                }
                return new KeyValuePair<bool, string>(valid, errorMessage);
            }

            return new KeyValuePair<bool, string>(false, "One of the workpieces is null");
        }



        #endregion

    }
}
