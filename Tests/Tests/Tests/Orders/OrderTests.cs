
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
    using global::Tests.Tests.Orders.Templates;
    using global::Tests.TestsData.Orders;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework;

    using OpenQA.Selenium;

    [TestFixture]
    [Parallelizable(ParallelScope.All)]
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
            var workpieceTitles = this.App.Ui.OrdersOrder.GetSectionFieldsNames("Add new workpieces");

            var states = this.App.Ui.OrdersOrder.GetCreateOrderFieldsStates();
            var placeholders = this.App.Ui.OrdersOrder.GetCreateOrderFieldsPlaceholders();

            this.App.Ui.OrdersOrder.CloseCreateOrderPopup();
            
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
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);

            var isSaveButtonEnabledCustomerIs = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            orderToAdd = testOrder.Generate();
            orderToAdd.ExternalOrderId = null;

            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            var isSaveButtonEnabledOrderId = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            orderToAdd = testOrder.Generate();
            orderToAdd.DeliveryDate = DateTime.Parse("0001-01-01", System.Globalization.CultureInfo.InvariantCulture);

            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
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

            var isSaveButtonEnabled = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            Assert.True(isSaveButtonEnabled, $"Save button should be enabled since '{field.ToString()}' is optional field");

            this.App.Ui.OrdersOrder.ClickSaveButton();

            var order = this.App.Ui.OrdersMain.FindOrder(orderToAdd.ExternalOrderId);

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
            var order = this.App.Ui.OrdersMain.GetOrderGridRecords().First();
            var orderId = order.OrderId;
            var customer = order.Customer;

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();

            var testOrder = new Faker<Order>()
                .RuleFor(o => o.ExternalOrderId, f => orderId)
                .RuleFor(o => o.CustomerId, f => OrdersData.Customers[customer])
                .RuleFor(o => o.Comments, f => f.Hacker.Phrase())
                .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));

            var testWorkpiece = this.WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();
            var workpieceToAdd = testWorkpiece.Generate();

            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);

            var isSaveButtonEnabled = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();

            Assert.False(isSaveButtonEnabled, $"Save button should be disabled since Order Id '{orderId}' is not unique");
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

            this.App.Ui.OrdersOrder.ClickSaveButton();

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
        [Property("TestCase", "717")]
        [Property("TestCase", "718")]
        [Property("TestCase", "719")]
        [Property("TestCase", "722")]
        [Property("TestCase", "723")]
        [Property("TestCase", "724")]
        [Property("TestCase", "728")]
        [Property("TestCase", "733")]
        [Property("TestCase", "741")]
        [Property("TestCase", "744")]
        [Property("TestCase", "750")]
        [Property("TestCase", "736")]
        [Property("TestCase", "737")]
        [Property("TestCase", "738")]
        [Property("TestCase", "739")]
        [Property("TestCase", "734")]

        public void AddValidWorkpiecesSpecificValues()
        {
            var testOrder = this.OrderGenerationRule;
            var testWorkpiece = new Faker<Workpiece>().Rules(
                (f, o) =>
                    {
                        o.Name = f.Hacker.Noun() + f.Finance.Amount() + "_^$^&$@)(~";
                        o.DeliveryDate = f.Date.Future(1);
                        o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation() + f.Finance.Amount() + "_^$^&$@)(~";
                        o.Quantity = f.Random.Int(2, 10000);
                    });

            var orderToAdd = testOrder.Generate();
           
            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            List<Workpiece> workpiecesToAdd = new List<Workpiece>();
            int numberOfWorkpieces = new Faker().PickRandom(3, 6);
            for (int i = 0; i < numberOfWorkpieces; i++)
            {
                var workpieceToAdd = testWorkpiece.Generate();
                this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);
                workpiecesToAdd.Add(workpieceToAdd);
            }

            var states = this.App.Ui.OrdersOrder.GetCreateOrderFieldsStates();

            this.App.Ui.OrdersOrder.ClickSaveButton();
           
            this.App.Ui.OrdersMain.OpenOrderDetailsDirectly(orderToAdd.ExternalOrderId);
            
            var createdWorkpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();

            Assert.True(workpiecesToAdd.Count.Equals(createdWorkpieces.Count));

            foreach (var wp in workpiecesToAdd)
            {
                var workpieceToCheck = createdWorkpieces.First(w => w.Id == wp.ExternalWorkpieceId);


                var workPiecesEqual = this.CompareWorkpieceWithWorkpieceFromGrid(workpieceToCheck, wp);
                
                Assert.True(workPiecesEqual.Key,
                    "Workpieces weren't created as populated." + Environment.NewLine + workPiecesEqual.Value);              
            }

            Assert.True(
                states[CreateOrderPopup.OrderFields.WorkpieceId].Equals(string.Empty)
                && states[CreateOrderPopup.OrderFields.WorkpieceName].Equals(string.Empty) && states[CreateOrderPopup.OrderFields.WorkpieceQuantity].Equals(string.Empty)
                        && states[CreateOrderPopup.OrderFields.WorkpieceDeliveryDate].Equals(orderToAdd.DeliveryDate.ToString("yyyy-MM-dd")),
                "Fields weren't reset after adding workpiece");
        }

        //Todo: add API validation test
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "716")]
        [Property("TestCase", "721")]
        [Property("TestCase", "726")]
        [Property("TestCase", "731")]
        public void CheckWorkpieceFieldsRequiredValidation()
        {
            var testOrder = this.OrderGenerationRule;

            var testWorkpiece = this.WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();
           
            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            // Check ID field
            var workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.ExternalWorkpieceId = null;
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledWpId = this.App.Ui.OrdersOrder.IsAddWorkpieceButtonEnabled();

            // Check Name field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.Name = string.Empty;

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledWpName = this.App.Ui.OrdersOrder.IsAddWorkpieceButtonEnabled();
            
            // Check Quantity field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.Quantity = null;

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledWpQuantity = this.App.Ui.OrdersOrder.IsAddWorkpieceButtonEnabled();

            // Check Delivery date field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.DeliveryDate = DateTime.Parse("0001-01-01");

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledDeliveryDate = this.App.Ui.OrdersOrder.IsAddWorkpieceButtonEnabled();

            Assert.Multiple(
                () =>
                    {
                        Assert.False(isAddWorkpieceButtonEnabledWpId, "Save button should be disabled since 'WorkpieceId' is a mandatory field");
                        Assert.False(isAddWorkpieceButtonEnabledWpName, "Save button should be disabled since 'Workpiece name' is a mandatory field");
                        Assert.False(isAddWorkpieceButtonEnabledWpQuantity, "Save button should be disabled since 'Quantity' is a mandatory field");
                        Assert.False(isAddWorkpieceButtonEnabledDeliveryDate, "Save button should be disabled since 'Delivery date' is a mandatory field");
                    });

            this.App.Ui.OrdersOrder.ClickSaveButton(false);
            Assert.True(this.App.Ui.OrdersOrder.IsCreateOrderPopUpOpened(), "Create order popup shouldn't be closed");
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
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            
            var isSaveButtonEnabled = this.App.Ui.OrdersOrder.IsSaveButtonEnabled();
            Assert.False(isSaveButtonEnabled, "Save order button should be disabled since at least one workpiece is not added");

            this.App.Ui.OrdersOrder.ClickSaveButton(false);
            var isCreateOrderPopUpOpened = this.App.Ui.OrdersOrder.IsCreateOrderPopUpOpened();
            Assert.True(isCreateOrderPopUpOpened, "Create order popup should be opened");
        }




        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "727")]
        [Property("TestCase", "729")]
        public void WorkpieceQuantityFieldInvalidSymbolsValidation()
        {
            var testOrder = this.OrderGenerationRule;
            var orderToAdd = testOrder.Generate();
            var testWorkpiece = this.WorkpieceGenerationRule;
            var workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.Quantity = null;
           
            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            this.App.Ui.OrdersOrder.PopulateField(CreateOrderPopup.OrderFields.WorkpieceQuantity, "Some text data");
            var states = this.App.Ui.OrdersOrder.GetCreateOrderFieldsStates();
            Assert.True(states[CreateOrderPopup.OrderFields.WorkpieceQuantity].Equals(string.Empty));
            this.App.Ui.OrdersOrder.PopulateField(CreateOrderPopup.OrderFields.WorkpieceQuantity, "^*&#^*(^@#^%");
            Assert.True(states[CreateOrderPopup.OrderFields.WorkpieceQuantity].Equals(string.Empty));
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "746")]
        [Property("TestCase", "747")]
        public void CheckWorkpieceDeliveryDateBehaviour()
        {
            var testOrder = this.OrderGenerationRule;
            var orderToAdd = testOrder.Generate();
            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var states = this.App.Ui.OrdersOrder.GetCreateOrderFieldsStates();

            Assert.True(
                orderToAdd.DeliveryDate.ToString("yyyy-MM-dd")
                    .Equals(states[CreateOrderPopup.OrderFields.WorkpieceDeliveryDate]),
                "Workpiece delivery date should be populated after order but was not");

            this.App.Ui.OrdersOrder.PopulateField(CreateOrderPopup.OrderFields.OrderDeliveryDate, "2020-12-12");

            Assert.True(
                orderToAdd.DeliveryDate.ToString("yyyy-MM-dd")
                    .Equals(states[CreateOrderPopup.OrderFields.WorkpieceDeliveryDate]),
                "Workpiece delivery date shouldn't be changed after order delivery date being updated");
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

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);
            var orderDetails = this.App.Ui.OrdersOrder.GetInfoFromOrderDetailsPage();

            Assert.True(orderToWork.Equals(orderDetails), "Data on the order details form is not correct");

            var dateFormatIsMatch = Regex.IsMatch(orderDetails.CreationDate, "\\d{1,2}/\\d{1,2}/\\d{4}");
            Assert.True(dateFormatIsMatch, "Date format doesn't match expected 'dd/MM/yyyy'");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-147")]
        [Property("Reference", "TLM-122")]
        [Property("TestCase", "684")]
        [Property("TestCase", "689")]
        [Property("TestCase", "735")]
        [Property("TestCase", "740")]
        public void CheckWorkpieceAttributes()
        {
            List<string> expectedColumnsNames = new List<string> { "ID", "Name", "Quantity", "Workpiece Delivery" };

            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                expectedColumnsNames = ServiceMethods.StringListToUpper(expectedColumnsNames);
            }


            Faker faker = new Faker();
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            var orderToWork = faker.PickRandom(orders);

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);
            var titles = this.App.Ui.OrdersOrder.GetWorkpiecesTableTitles();

            var workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();
            var workpieceToWork = faker.PickRandom(workpieces);

            var dateFormatIsMatch = Regex.IsMatch(workpieceToWork.DeliveryDate, "\\d{1,2}/\\d{1,2}/\\d{4}");
            Assert.Multiple(
                () =>
                    {
                        Assert.True(expectedColumnsNames.SequenceEqual(titles), "Workpiece titles are incorrect");
                        Assert.True(dateFormatIsMatch, "Date format doesn't match expected 'dd/MM/yyyy'");
                    });
        }


        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-147")]
        [Property("Reference", "TLM-123")]
        [Property("TestCase", "690")]
        [Property("TestCase", "691")]
        [Property("TestCase", "752")]
        [Property("TestCase", "751")]
        [Property("TestCase", "753")]

        public void OpenWorkplanPageAndBack()
        {
            List<string> expectedTaskGridColumnsNames =
                new List<string>
                    {
                        "Name",
                        "Duration per Workpiece",
                        "Duration in Total",
                        "Start",
                        "End",
                        "Machine",
                        "CAM File"
                    };

            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                expectedTaskGridColumnsNames = ServiceMethods.StringListToUpper(expectedTaskGridColumnsNames);
            }

            List<string> expectedNewTaskInputFields = new List<string>
                                                          {
                                                              "Name:",
                                                              "Duration per Workpiece:",
                                                              "Duration in Total:",
                                                              "Start:",
                                                              "End:",
                                                              "Machine:"
                                                          };


            Faker faker = new Faker();
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            var orderToWork = faker.PickRandom(orders);

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);

            var workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();
            var workpieceToWork = faker.PickRandom(workpieces);

            this.App.Ui.OrdersOrder.ClickWorkpiece(workpieceToWork.Id);
            var columnsNames = this.App.Ui.OrdersWorkpiece.GetTasksColumnsNames();
            var filedsNames = this.App.Ui.OrdersWorkpiece.GetTaskInputFieldsNames();
                
            this.App.Ui.OrdersWorkpiece.ClickBackLink();

            workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords().Where(r => r.Id.Equals(workpieceToWork.Id)).ToList();

            Assert.True(workpieces.Count == 1, "We weren't returned to order details page");

            this.App.Ui.OrdersOrder.ClickBackLink();

            orders = this.App.Ui.OrdersMain.GetOrderGridRecords().Where(o => o.OrderId.Equals(orderToWork.OrderId))
                .ToList();

            Assert.True(orders.Count == 1, "We weren't returned to orders page");

            Assert.Multiple(
                () =>
                    {
                        Assert.True(expectedTaskGridColumnsNames.SequenceEqual(columnsNames), "Tasks table columns titles are incorrect");
                        Assert.True(expectedNewTaskInputFields.SequenceEqual(filedsNames), "New task input field titles are incorrect"); 
                    });
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-123")]
        [Property("TestCase", "755")]
        [Property("TestCase", "756")]
        [Property("TestCase", "758")]
        [Property("TestCase", "759")]
        public void CheckWorkpieceDataOnDetails()
        {
            Faker faker = new Faker();
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            var orderToWork = faker.PickRandom(orders);

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);

            var workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();
            var workpieceToWork = faker.PickRandom(workpieces);

            this.App.Ui.OrdersOrder.ClickWorkpiece(workpieceToWork.Id);

            var workpiece = this.App.Ui.OrdersWorkpiece.GetWorkpieceData();

            var workpieceDataIsCorrect = this.CompareWorkpieceWithWorkpieceFromGrid(workpieceToWork, workpiece);

            Assert.That(workpieceDataIsCorrect.Key, workpieceDataIsCorrect.Value);
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-123")]
        [Property("TestCase", "761")]
        [Property("TestCase", "766")]
        [Property("TestCase", "771")]
        [Property("TestCase", "776")]
        [Property("TestCase", "779")]
        [Property("TestCase", "783")]
        [Property("TestCase", "787")]
        [Property("TestCase", "794")]
        [Property("Bug", "TLM-286")]

        public void CheckWorkplanDataFieldsInitialState()
        {
            Faker faker = new Faker();
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            var orderToWork = faker.PickRandom(orders);

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);

            var workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();
            var workpieceToWork = faker.PickRandom(workpieces);

            this.App.Ui.OrdersOrder.ClickWorkpiece(workpieceToWork.Id);

            var states = this.App.Ui.OrdersWorkpiece.GetTaskInputFieldsStates();

            foreach (var state in states)
            {
                if (!state.Key.Equals(WorkpieceDetails.TaskFields.Machine))
                    Assert.True(
                        state.Value.Equals(string.Empty),
                        $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected empty");
                else
                {
                    Assert.True(state.Value.Equals("Select Machine"), $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected 'Select Machine'");
                }
            }
           

            var placeholders = this.App.Ui.OrdersWorkpiece.GetNewTaskFieldsPlaceholders();

            Assert.Multiple(
                () =>
                    {
                        Assert.True(
                        placeholders[WorkpieceDetails.TaskFields.Start].Equals("yyyy-mm-dd"),
                        "Start field placeholder is not correct");
                        Assert.True(
                            placeholders[WorkpieceDetails.TaskFields.End].Equals("yyyy-mm-dd"),
                            "End field placeholder is not correct");
                        });
        }


        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-123")]
        [Property("TestCase", "802")]
        [Property("TestCase", "803")]
        [Property("TestCase", "804")]
        [Property("TestCase", "806")]
        [Property("TestCase", "807")]
        [Property("TestCase", "808")]
        [Property("TestCase", "811")]
        [Property("TestCase", "812")]
        [Property("TestCase", "798")]
        [Property("TestCase", "799")]
        public void CheckTaskCreation()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

            var task = this.TaskGenerationRule.Generate();

            this.App.Ui.OrdersWorkpiece.AddNewTask(task);
            var tasks = this.App.Ui.OrdersWorkpiece.GetTasksRecords();
            var addedTask = tasks.First(t => t.Name.Equals(task.Name));

            Assert.Multiple(
                () =>
                    {
                        Assert.True(tasks.Count.Equals(1), "Task wasn't created or more than 1 tasks were created");
                        Assert.True(
                            ServiceMethods.ConvertDurationFromTimeFormatToSeconds(addedTask.DurationPerWorkpiece)
                                .Equals(task.DurationPerWorkpiece),
                            $"Duration per workpieces is incorrect. Expected '{task.DurationPerWorkpiece}' but actual '{ServiceMethods.ConvertDurationFromTimeFormatToSeconds(addedTask.DurationPerWorkpiece)}'");
                        Assert.True(
                            ServiceMethods.ConvertDurationFromTimeFormatToSeconds(addedTask.DurationInTotal)
                                .Equals(task.DurationPerTotal),
                            $"Duration per workpieces is incorrect. Expected '{task.DurationPerTotal}' but actual '{ ServiceMethods.ConvertDurationFromTimeFormatToSeconds(addedTask.DurationInTotal)}'");
                        Assert.True(addedTask.Start.Equals(task.StartDate.ToString("MM/dd/yyyy")), $"Start date is incorrect. Expected '{task.StartDate:MM/dd/yyyy}' but actual '{addedTask.Start}'");
                        Assert.True(addedTask.End.Equals(task.EndDate.ToString("MM/dd/yyyy")), $"End date is incorrrect. Expected '{task.EndDate:MM/dd/yyyy}' but actual '{addedTask.End}'");

                        var machineName = this.App.GraphApi.ProjectManager.GetMachine(task.MachineId).name;
                        Assert.True(addedTask.Machine.Equals(machineName), $"Machine name is incorrect. Expected '{machineName}' but actual was '{addedTask.Machine}'");
                    });

            // Check input fields states were reset
            var states = this.App.Ui.OrdersWorkpiece.GetTaskInputFieldsStates();
            foreach (var state in states)
            {
                if (!state.Key.Equals(WorkpieceDetails.TaskFields.Machine))
                    Assert.True(
                        state.Value.Equals(string.Empty),
                        $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected empty");
                else
                {
                    Assert.True(state.Value.Equals("Select Machine"), $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected 'Select Machine'");
                }
            }
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-123")]
        [Property("TestCase", "813")]
        [Property("TestCase", "815")]
        public void AddSeveralTasksAndCheckWorkplan()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

            Faker f = new Faker();
            int numberOfTasks = f.Random.Int(3, 6);

            List<Task> tasksToAdd = new List<Task>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                var task = this.TaskGenerationRule.Generate();
                this.App.Ui.OrdersWorkpiece.AddNewTask(task);
                tasksToAdd.Add(task);
            }
            
            var tasksUi = this.App.Ui.OrdersWorkpiece.GetTasksRecords();

            Assert.True(tasksToAdd.Count.Equals(tasksUi.Count), "Not all needed tasks were created");

//            App.Ui.Main.NavigateUsingBreadcrumbs(2);
//            var workpiece = this.App.GraphApi.ProjectManager.GetWorkpiece(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);
//            this.App.Ui.OrdersOrder.ClickWorkpiece(workpiece.ExternalWorkpieceId);

            var tasksUiAfterNavigation = this.App.Ui.OrdersWorkpiece.GetTasksRecords();

            Assert.True(tasksUi.SequenceEqual(tasksUiAfterNavigation), "Created tasks are not displayed correctly");

            var namesOfTasksToCreate = tasksToAdd.Select(t => t.Name).ToList();
            
            var namesOfCreatedTasks = tasksUi.Select(t => t.Name).ToList();

            Assert.True(namesOfTasksToCreate.SequenceEqual(namesOfCreatedTasks), "Tasks were not added properly");
        }

        [Test]
        [Category("UI")]
        [Category("work")]
        [Property("Reference", "TLM-130")]
        [Property("TestCase", "1239")]
        [Property("TestCase", "1242")]
        [Property("TestCase", "1245")]
        public void CheckValidCamFileUpload()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan]);

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);
            
            var tasks = App.Ui.OrdersWorkpiece.GetTasksRecords();
            var taskName = tasks.First().Name;
            var filename = "ValidCamFile.h";
            string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + filename;
            App.Ui.OrdersWorkpiece.AddCamFileForTask(taskName, path);
            var task = App.Ui.OrdersWorkpiece.GetTasksRecords().First(t => t.Name.Equals(taskName));
            
            Assert.True(task.CamFile.Equals(filename), "Added cam file is not displayed for task");
            var filename2 = "ValidCamFile2.h";
            path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + filename2;

            Exception exc = null;
            try
            {
                App.Pages.Driver.DisableTimeout();
                App.Ui.OrdersWorkpiece.AddCamFileForTask(taskName, path, false);
            }
            catch (NoSuchElementException e)
            {
                exc = e;
            }
            finally
            {
                App.Pages.Driver.SetTimeout();
            }

            Assert.True(exc != null, "It shouldn't be possible to add second cam file");

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);
            task = App.Ui.OrdersWorkpiece.GetTasksRecords().First(t => t.Name.Equals(taskName));

            Assert.True(task.CamFile.Equals(filename), "Added cam file is not displayed for task");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-130")]
        [Property("TestCase", "1243")]
        public void CheckInvalidCamFileUpload()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan]);

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

            var tasks = App.Ui.OrdersWorkpiece.GetTasksRecords();
            var taskName = tasks.First().Name;
            var filename = "InValidCamFile.hh";
            string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + filename;

            string message = string.Empty;
            try
            {
                App.Ui.OrdersWorkpiece.AddCamFileForTask(taskName, path);
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            Assert.True(message.Equals("Can't add cam file"), "Invalid cam file could be added");

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);
            
            var task = App.Ui.OrdersWorkpiece.GetTasksRecords().First(t => t.Name.Equals(taskName));

            Assert.True(task.CamFile.ToLower().Equals("add cam file"), "Add cam file button is not displayed");
        }

        //ToDo: Add more filetypes
        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-163")]
        [Property("Reference", "TLM-168")]
        [Property("TestCase", "1759")]
        [Property("TestCase", "358")]
        [Property("TestCase", "362")]
        [Property("TestCase", "1779")]
        [Property("TestCase", "371")]
        public void CheckWorkpiceFileUpload()
        {
            var testOrder = this.OrderGenerationRule;
            var testWorkpiece = new Faker<Workpiece>().Rules(
                (f, o) =>
                    {
                        o.Name = f.Hacker.Noun() + f.Finance.Amount();
                        o.DeliveryDate = f.Date.Future(1);
                        o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation() + f.Finance.Amount();
                        o.Quantity = f.Random.Int(2, 10000);
                    });

            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var files = new List<string> { "Length_sorting.png", "Test upload file.docx" };

            var filePaths = new List<string>();

            foreach (var file in files)
            {
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + file;
                  filePaths.Add(path);
            }
           
            var workpieceToAdd = testWorkpiece.Generate();
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd); 
            App.Ui.OrdersOrder.AddFilesForWorkpiece(workpieceToAdd.ExternalWorkpieceId, filePaths);
            var fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece(workpieceToAdd.ExternalWorkpieceId);

            Assert.True(fileNames.Count == filePaths.Count && fileNames.SequenceEqual(files), $"Expected '{ServiceMethods.ListToString(files)}' but actual '{ServiceMethods.ListToString(fileNames)}'");

        }



        #region Generation rules
        private Faker<Order> OrderGenerationRule => new Faker<Order>()
            .RuleFor(o => o.ExternalOrderId, f => "AutoOrder " + f.Company.CompanyName() + "_" + f.Finance.Amount(1, 10000))
            .RuleFor(o => o.CustomerId, f => f.PickRandom(OrdersData.Customers.Values.ToList()))
            .RuleFor(o => o.Comments, f => f.Hacker.Phrase())
            .RuleFor(o => o.DeliveryDate, f => f.Date.Future(1));


        private Faker<Workpiece> WorkpieceGenerationRule => new Faker<Workpiece>().Rules(
            (f, o) =>
                {
                    o.Name = f.Hacker.Noun();
                    o.DeliveryDate = f.Date.Future(1);
                    o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation();
                    o.Quantity = f.Random.Int(2, 10000);
                });

        private Faker<Task> TaskGenerationRule => new Faker<Task>().Rules(
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
        private KeyValuePair<bool, string> CompareOrderWithOrderFromGrid(Order order, Order orderExpected)
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

        private KeyValuePair<bool, string> CompareWorkpieceWithWorkpieceFromGrid(WorkpieceGridRecord workpieceToCheck, Workpiece wpExpected)
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
