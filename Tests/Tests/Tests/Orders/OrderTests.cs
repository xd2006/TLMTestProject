
namespace Tests.Tests.Orders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bogus;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.Tests.Orders.Templates;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework;

    using OpenQA.Selenium;

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("Order")]
   
    public class OrderTests : OrdersTestTemplate
    {
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-152")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "347")]
        [Property("TestCase", "348")]
        [Property("TestCase", "349")]
        [Property("TestCase", "658")]
        [Property("TestCase", "661")]
        [Property("TestCase", "664")]
        [Property("TestCase", "666")]
        [Property("TestCase", "714")]
        [Property("TestCase", "715")]
        [Property("TestCase", "720")]
        [Property("TestCase", "725")]
        [Property("TestCase", "730")]

        public void CheckCreateOrderPopup()
        {

            List<string> addAdditionalInfoTitles = new List<string> { "Customer:", "Order ID:", "Order Delivery Date:", "Comments:" };
            List<string> addNewWorkpiecesTitles = new List<string> { "Workpiece ID:", "Workpiece name:", "Quantity:", "Workpiece Delivery Date:" };

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            var infoTitles = this.App.Ui.OrdersOrder.GetOrderFieldsNames();
            var states = this.App.Ui.OrdersOrder.GetCreateOrderFieldsStates();
            var placeholders = this.App.Ui.OrdersOrder.GetCreateOrderFieldsPlaceholders();

            var order = OrderGenerationRule.Generate();
            App.Ui.OrdersOrder.PopulateOrderData(order);
            App.Ui.OrdersOrder.ClickAddWorkpieceButton();

            var workpieceTitles = App.Ui.OrdersWorkpiece.GetWorkpieceFieldsNames();
            var placeHoldersWorkpiece = this.App.Ui.OrdersWorkpiece.GetCreateWorkpieceFieldsPlaceholders();
            


            Assert.Multiple(
                () =>
                {
                    Assert.True(infoTitles.SequenceEqual(addAdditionalInfoTitles), "Order info fields titles are wrong");
                    Assert.True(
                        workpieceTitles.SequenceEqual(addNewWorkpiecesTitles),
                        "Workpiece fields titles are wrong");
                    Assert.True(
                        states.Values.All(v => v.Equals(string.Empty)),
                        "Not all fields are in initial state");
                    Assert.True(
                        placeholders.Values.All(p => p.Equals("yyyy-mm-dd")),
                        "Not all placeholders are correct");
                    Assert.True(
                        placeHoldersWorkpiece.Values.All(p => p.Equals("yyyy-mm-dd")),
                        "Not all placeholders are correct");
                });
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-152")]
        [Property("TestCase", "350")]
        [Property("TestCase", "662")]
        [Property("TestCase", "667")]
        public void CheckCreateOrderFieldsRequiredValidation()
        {
            var testOrder = this.OrderGenerationRule;

            var testWorkpiece = this.WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();
            orderToAdd.CustomerId = null;

            var workpieceToAdd = testWorkpiece.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd, true);

            var isSaveButtonEnabledCustomerIs = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            orderToAdd = testOrder.Generate();
            orderToAdd.ExternalOrderId = null;

            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd, true);
            var isSaveButtonEnabledOrderId = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            orderToAdd = testOrder.Generate();
            orderToAdd.DeliveryDate = DateTime.Parse("0001-01-01", System.Globalization.CultureInfo.InvariantCulture);

            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd, true);
            var isSaveButtonDeliveryDate = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();


            Assert.Multiple(
                () =>
                {
                    Assert.False(isSaveButtonEnabledCustomerIs, "Save button should be disabled since CustomerId is a mandatory field");
                    Assert.False(isSaveButtonEnabledOrderId, "Save button should be disabled since OrderId is a mandatory field");
                    Assert.False(isSaveButtonDeliveryDate, "Save button should be disabled since DeliveryDate is a mandatory field");
                });
            this.App.Ui.OrdersOrder.ClickSaveButton(false);
            Assert.True(this.App.Ui.OrdersOrder.IsCreateOrderPopUpOpened(), "Create order popup shouldn't be closed");
        }

        [Test, TestCaseSource(typeof(FieldsVerificationDataSource), nameof(FieldsVerificationDataSource.OptionalFieldsTestCases))]
        [Category("UI")]
        [Property("Reference", "TLM-152")]
       public void CheckCreateOrderFieldsOptionalValidation(CreateOrderPopup.OrderFields field)
        {
            var testOrder = new Faker<Order>()
                .RuleFor(
                    o => o.ExternalOrderId,
                    f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000)).RuleFor(
                    o => o.CustomerId,
                    f => f.PickRandom(OrdersData.Customers.Values.ToList()))               
                    .RuleFor(o => o.Comments, f => f.Hacker.Phrase());

            switch (field)
            {
                case CreateOrderPopup.OrderFields.Comments:
                    {
                        testOrder.RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));
                        break;
                    }
                    
                default:
                    {
                        testOrder.RuleFor(o => o.Comments, f => f.Hacker.Phrase())
                            .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));
                        break;
                    }
            }

            var testWorkpiece = this.WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();

            var workpieceToAdd = testWorkpiece.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);

            App.Ui.OrdersOrder.ClickBackLink();
            var order = this.App.Ui.OrdersMain.FindOrder(orderToAdd.ExternalOrderId, true);

            var ordersAreEqual = this.CompareOrderWithOrderFromGrid(order, orderToAdd);

            Assert.True(ordersAreEqual.Key, "Created order contains some wrong data: " + Environment.NewLine + ordersAreEqual.Value);
        }


        //ToDo: create API test for server side validation 
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-152")]
        [Property("TestCase", "663")]
        public void CheckOrderIdUniqueValidation()
        {
            var order = this.App.Ui.OrdersMain.GetOrderGridRecords(true, 50).First();
            var orderId = order.OrderId;
            var customer = order.Customer;

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();

            var testOrder = new Faker<Order>()
                .RuleFor(o => o.ExternalOrderId, f => orderId)
                .RuleFor(o => o.CustomerId, f => OrdersData.Customers[customer])
                .RuleFor(o => o.Comments, f => f.Hacker.Phrase())
                .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));
            
            var orderToAdd = testOrder.Generate();
            
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd, true);
            var isSaveButtonDisabled = App.Ui.OrdersOrder.SaveButtonShouldBeDisabled();
            
            Assert.True(isSaveButtonDisabled, $"Save button should be disabled since Order Id '{orderId}' is not unique");
            this.App.Ui.OrdersOrder.ClickSaveButton(false);
            Assert.True(this.App.Ui.OrdersOrder.IsCreateOrderPopUpOpened(), "Create order popup shouldn't be closed");           
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-152")]
        [Property("TestCase", "352")]
        [Property("TestCase", "353")]
        [Property("TestCase", "354")]
        [Property("TestCase", "355")]
        [Property("TestCase", "356")]
        [Property("TestCase", "357")]
        [Property("TestCase", "373")]
        [Property("TestCase", "374")]
        public void CheckOrderFieldsSpecificData()
        {
            var testOrder = new Faker<Order>()
                .RuleFor(o => o.ExternalOrderId, f => f.Hacker.Adjective() + f.Finance.Amount() + "_^$^&$@)(~")
                .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
                .RuleFor(o => o.Comments, f => f.Hacker.Adjective() + f.Finance.Amount() + "_^$^&$@)(~")
                .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));

            var testWorkpiece = this.WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();
            var workpieceToAdd = testWorkpiece.Generate();
            
            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);

            App.Ui.OrdersOrder.ClickBackLink();

            var orderUi = this.App.Ui.OrdersMain.FindOrder(orderToAdd.ExternalOrderId, true);
            var orderApi = this.App.GraphApi.ProjectManager.GetOrders().First(o => o.ExternalOrderId.Equals(orderToAdd.ExternalOrderId));

            Assert.True(
                orderToAdd.ExternalOrderId.Equals(orderUi.ExternalOrderId)
                && orderToAdd.Comments.Equals(orderApi.Comments),
                "Order data was saved wrongly");
        }
        
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "743")]
        public void AddOrderWithoutWorkpiece()
        {
            var testOrder = this.OrderGenerationRule;
            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd, true);
            
            var isSaveButtonEnabled = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();
            Assert.True(isSaveButtonEnabled, "Save order button should be enabled");

            this.App.Ui.OrdersOrder.ClickSaveButton();
            var isCreateOrderPopUpOpened = this.App.Ui.OrdersOrder.IsCreateOrderPopUpOpened();
            Assert.False(isCreateOrderPopUpOpened, "Create order popup shouldn't be opened");
        }
        
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-147")]
        [Property("TestCase", "673")]
        [Property("TestCase", "674")]
        [Property("TestCase", "676")]
        [Property("TestCase", "677")]
        [Property("TestCase", "678")]
        [Property("TestCase", "679")] 
        [Property("TestCase", "680")]
        [Property("TestCase", "681")]
        [Property("TestCase", "682")]
        [Property("TestCase", "683")]
        
        public void CheckOrderDetailsScreen()
        {
            #region get reference data

            Faker faker = new Faker();
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            var orderToWork = faker.PickRandom(orders);
           
            #endregion

            this.App.Ui.OrdersMain.OpenOrderDetailsDirectly(orderToWork.OrderId);
            var orderDetails = this.App.Ui.OrdersOrder.GetInfoFromOrderDetailsPage();

            Assert.True(orderToWork.Equals(orderDetails), "Data on the order details form is not correct");

            var dateFormatIsMatch = Regex.IsMatch(orderDetails.CreationDate, "\\d{1,2}/\\d{1,2}/\\d{4}");
            Assert.True(dateFormatIsMatch, "Date format doesn't match expected 'dd/MM/yyyy'");
        }
        
       public class FieldsVerificationDataSource
        {
            public static IEnumerable OptionalFieldsTestCases
            {
                get
                {
                    yield return new TestCaseData(CreateOrderPopup.OrderFields.Comments)
                        .SetProperty("TestCase", "665");
                }
            }
        }
    }
}
