using Core.Utils;

namespace Tests.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bogus;

    using Core.Service;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Tests.Orders.Templates;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework;
    

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("Workpiece")]
    public class OrderWorkpieceTests : OrdersTestTemplate
    {
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

            var materials = App.GraphApi.ToolManager.GetUsageMaterials();

            var testOrder = OrderGenerationRule;
            var testWorkpiece = new Faker<Workpiece>().Rules(
                (f, o) =>
                {
                    o.Name = f.Hacker.Noun() + f.Finance.Amount() + "_^$^&$@)(~";
                    o.DeliveryDate = f.Date.Future(1);
                    o.ExternalWorkpieceId = f.Hacker.Noun() + "_" + f.Hacker.Abbreviation() + f.Finance.Amount()
                                            + "_^$^&$@)(~";
                    o.RawMaterialId = int.Parse(f.PickRandom(materials.Select(m => m.Id).ToList()));
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
                workpiecesToAdd.Add(workpieceToAdd);
                this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd);
            }

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

            var isAddWorkpieceButtonEnabledWpId = this.App.Ui.OrdersWorkpiece.IsSaveWorkpieceButtonEnabled();

            // Check Name field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.Name = string.Empty;

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledWpName = this.App.Ui.OrdersWorkpiece.IsSaveWorkpieceButtonEnabled();

            // Check Quantity field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.Quantity = null;

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledWpQuantity = this.App.Ui.OrdersWorkpiece.IsSaveWorkpieceButtonEnabled();

            // Check Delivery date field
            workpieceToAdd = testWorkpiece.Generate();
            workpieceToAdd.DeliveryDate = DateTime.Parse("0001-01-01");

            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);

            var isAddWorkpieceButtonEnabledDeliveryDate = this.App.Ui.OrdersWorkpiece.IsSaveWorkpieceButtonEnabled();

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
            this.App.Ui.OrdersWorkpiece.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity, "Some text data");
            var states = this.App.Ui.OrdersWorkpiece.GetWorkpieceInputFieldsStates();
            Assert.True(states[CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity].Equals(string.Empty));
            this.App.Ui.OrdersWorkpiece.PopulateField(CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity, "^*&#^*(^@#^%");
            Assert.True(states[CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity].Equals(string.Empty));
        }


        [Test]
        [Ignore("Outdated")]
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

            //            Assert.True(
            //                orderToAdd.DeliveryDate.ToString("yyyy-MM-dd")
            //                    .Equals(states[CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate]),
            //                "Workpiece delivery date should be populated after order but was not");

            this.App.Ui.OrdersOrder.PopulateField(CreateOrderPopup.OrderFields.OrderDeliveryDate, "2020-12-12");

            //            Assert.True(
            //                orderToAdd.DeliveryDate.ToString("yyyy-MM-dd")
            //                    .Equals(states[CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate]),
            //                "Workpiece delivery date shouldn't be changed after order delivery date being updated");
        }

        [Test]
        [Category("UI")]
        [Category("Work")]
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

            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords(true, 50);

            // temp
            App.Logger.Info($"Number of grid records {orders.Count}");

            var faker = new Faker();

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
        [Retry(2)]
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
                        "Duration Total",
                        "Machine",
                        "Start date",
                        "End date",
                        "NC File"
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


            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords(true, 50);
            Faker faker = new Faker();

            //temp
            if (orders.Count == 0)
            {
                App.Logger.Warn("Orders weren't found");
            }

            var orderToWork = faker.PickRandom(orders);

            this.App.Ui.OrdersMain.ClickOrder(orderToWork.OrderId);

            var workpieces = this.App.Ui.OrdersOrder.GetWorkpieceGridRecords();
            var workpieceToWork = faker.PickRandom(workpieces);

            this.App.Ui.OrdersOrder.ClickWorkpiece(workpieceToWork.Id);
            var columnsNames = this.App.Ui.OrdersWorkpiece.GetTasksColumnsNames();
            App.Ui.OrdersWorkpiece.ClickCreateNewTaskButton();
            var filedsNames = this.App.Ui.OrdersWorkpiece.GetTaskInputFieldsNames();

            this.App.Ui.OrdersTask.CloseAddTaskPopup();
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
            var orders = this.App.Ui.OrdersMain.GetOrderGridRecords(true, 50);
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
        [Category("NoEdge")]
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
            var testWorkpiece = WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var files = new List<string>
                            {
                                "Length_sorting.png",
                                "Length_sorting.bmp",
                                "Length_sorting.jpg",
                                "Length_sorting.jpeg",
                                "Test upload file.docx",
                                "Test upload file.doc",
                                "RP_PDF_Report.pdf",
                                "RP_Report.ppt",
                                "RP_Report.pptx",
                                "Test upload file - S.stp",
                                "Test upload file.pst",
                                "Length_sorting T.tif",
                                "Length_sorting TT.tiff",
                                "Test XLS.xls",
                                "Test XLSX.xlsx"
                            };

            var filePaths = new List<string>();

            foreach (var file in files)
            {
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + file;
                filePaths.Add(path);
            }

            var workpieceToAdd = testWorkpiece.Generate();
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            App.Ui.OrdersWorkpiece.AddFilesForWorkpiece(filePaths);
            var fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece();

            Assert.True(
                fileNames.Count == filePaths.Count && fileNames.SequenceEqual(files),
                $"Expected '{ServiceMethods.ListToString(files)}' but actual '{ServiceMethods.ListToString(fileNames)}'");
        }

        [Test]
        [Category("UI")]
        [Category("NoEdge")]
        [Category("ConsoleErrorExpected")]
        [Property("Reference", "TLM-163")]
        [Property("TestCase", "368")]
        
        public void CheckWorkpiceInvalidFileUpload()
        {
            var testOrder = this.OrderGenerationRule;
            var testWorkpiece = WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var files = new List<string>
                            {
                                "MicrosoftWebDriver.exe",
                                "MicrosoftWebDriver.bin",
                                "MicrosoftWebDriver.com",
                                "TestFileBat.bat",
                            };

            var filePaths = new List<string>();

            foreach (var file in files)
            {
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + file;
                filePaths.Add(path);
            }

            var workpieceToAdd = testWorkpiece.Generate();
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            App.Ui.OrdersWorkpiece.AddFilesForWorkpiece(filePaths);
            var fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece();

            Assert.True(
                fileNames.Count == 0,
                $"No added but actual '{ServiceMethods.ListToString(fileNames)}'");

            var logs = App.Ui.UiHelp.GetBrowserLogs();

            foreach (var file in files)
            {
                Assert.True(
                    logs.Any(
                        l => l.Message.Contains(
                            $"failed to attach file '{file}', reason: {file} has an invalid extension.")),
                    $"Exception for '{file}' file wasn't found in console");
            }
        }

        [Test]
        [Category("UI")]
        [Category("NoEdge")]
        [Category("ConsoleErrorExpected")]
        [Property("Reference", "TLM-163")]
        [Property("TestCase", "369")]
        public void CheckWorkpiceInvalidFileSizeUpload()
        {
            var testOrder = this.OrderGenerationRule;
            var testWorkpiece = WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var file = "logs.png";

            string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + file;

            var workpieceToAdd = testWorkpiece.Generate();
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            App.Ui.OrdersWorkpiece.AddFilesForWorkpiece(new List<string> { path });
            var fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece();

            Assert.True(
                fileNames.Count == 0,
                $"No added but actual '{ServiceMethods.ListToString(fileNames)}'");

            var logs = App.Ui.UiHelp.GetBrowserLogs();

            Assert.True(
                     logs.Any(
                         l => l.Message.Contains(
                             $"failed to attach file '{file}', reason: {file} is too large")),
                     $"Exception for '{file}' file wasn't found in console");
        }

        [Test]
        [Category("UI")]
        [Category("NoEdge")]
        [Property("Reference", "TLM-163")]
        [Property("TestCase", "372")]
        public void CheckWorkpiceFileDeletion()
        {
            var testOrder = this.OrderGenerationRule;
            var testWorkpiece = WorkpieceGenerationRule;

            var orderToAdd = testOrder.Generate();

            this.App.Ui.OrdersMain.ClickCreateNewOrderButton();
            this.App.Ui.OrdersOrder.PopulateOrderData(orderToAdd);

            var files = new List<string>
                            {
                                "Length_sorting.png",
                                "Length_sorting.bmp",
                                "Length_sorting.jpg",
                                "Length_sorting.jpeg",
                                "Test upload file.docx",
                                "Test upload file.doc",
                                "RP_PDF_Report.pdf",
                                "RP_Report.ppt",
                                "Length_sorting T.tif",
                                "Length_sorting TT.tiff",
                                "Test XLS.xls",
                                "Test XLSX.xlsx"
                            };

            var filePaths = new List<string>();

            foreach (var file in files)
            {
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + file;
                filePaths.Add(path);
            }

            var workpieceToAdd = testWorkpiece.Generate();
            this.App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            App.Ui.OrdersWorkpiece.AddFilesForWorkpiece(filePaths);
            var fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece();

            Assert.True(
                fileNames.Count == filePaths.Count && fileNames.SequenceEqual(files),
                $"Expected '{ServiceMethods.ListToString(files)}' but actual '{ServiceMethods.ListToString(fileNames)}'");

            Faker f = new Faker();

            var number = f.Random.Int(2, files.Count - 3);
            var filesToDelete = f.PickRandom(files, number).ToList();

            filesToDelete.ForEach(e => files.Remove(e));

            App.Ui.OrdersWorkpiece.DeleteFilesForWorkpiece(filesToDelete);
            fileNames = App.Ui.OrdersOrder.GetFilesForWorkpiece();

            Assert.True(
                fileNames.Count == files.Count && fileNames.SequenceEqual(files),
                $"Expected '{ServiceMethods.ListToString(files)}' but actual '{ServiceMethods.ListToString(fileNames)}'");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-177")]
        [Property("TestCase", "1763")]
        [Property("TestCase", "1764")]
        [Property("TestCase", "1765")]
        public void OrderWorkpieceDetails_EmptyFilesGrid()
        {
            #region parameters
            var expectedTabNamesList = new List<string> { "general", "attached files" };
            var expectedColumnNames = new List<string> { "FILE NAME", "EDITOR", "CREATION DATE" };
            #endregion

            var orderId = App.Preconditions.CreateOrderWorkpieceWorkplan()[GeneralData.ProjectManagerEnitityTypes.Order];
            App.Ui.OrdersMain.OpenOrderDetailsDirectly(orderId);
            App.Ui.OrdersOrder.ClickRandomWorkpiece();
            var tabNameList = App.Ui.OrdersWorkpiece.GetTabListForWorkpieceDetailes();
            Assert.That(tabNameList.SequenceEqual(expectedTabNamesList), "Incorrect tabs for workpiece detailes");

            App.Ui.OrdersWorkpiece.NavigateToTab(WorkpieceDetails.WorkpieceDetailsTabs.AttachedFiles);

            var filesColumnNames = App.Ui.OrdersWorkpiece.GetFilesGridColumnNames;
            if (Parameters.Parameters.Browser == "MicrosoftEdge")
            {
                filesColumnNames = ServiceMethods.StringListToUpper(filesColumnNames);
            }

            Assert.That(filesColumnNames.SequenceEqual(expectedColumnNames), "Incorrect column in the Files grid");
            Assert.IsEmpty(App.Ui.OrdersWorkpiece.GetFilesGridRecords, "Files table is not empty");
        }

        [Test]
        [Category("UI")]
        [Category("NoEdge")]
        [Property("Reference", "TLM-177")]
        [Property("TestCase", "1766")]
        [Property("TestCase", "1781")]
        [Property("TestCase", "1782")]
        [Property("TestCase", "1783")]
        [Property("TestCase", "1785")]
        public void OrderWorkpieceDetails_FilesGridWithAttachedFiles()
        {
            #region parameters  
            var dateFormat = "MM/dd/yyyy h:mm:ss tt";
            var editor = "ProjectManager.Api";
            var projectPath = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\";
            var files = new List<string>
            {
                "Test upload file.doc",
                "Test XLS.xls",
                "Test XLSX.xlsx"
            };
            var filePaths = new List<string>();
            foreach (var file in files)
            {
                filePaths.Add(projectPath + file);
            }
            #endregion

            var orderId = App.Preconditions.CreateOrder();
            App.Ui.OrdersMain.OpenOrderDetailsDirectly(orderId);

            var testWorkpiece = this.WorkpieceGenerationRule;
            var workpieceToAdd = testWorkpiece.Generate();
            App.Ui.OrdersOrder.AddWorkpieceToOrder(workpieceToAdd, true);
            App.Ui.OrdersWorkpiece.AddFilesForWorkpiece(filePaths);
            App.Ui.OrdersWorkpiece.ClickSaveButton();
            var expectedTime = DateTime.Now;
            App.Ui.OrdersOrder.ClickRandomWorkpiece();
            App.Ui.OrdersWorkpiece.NavigateToTab(WorkpieceDetails.WorkpieceDetailsTabs.AttachedFiles);
            var filesRecords = App.Ui.OrdersWorkpiece.GetFilesGridRecords;
            Assert.AreEqual(filePaths.Count, filesRecords.Count, "Incorrect number of files");
            Assert.That(ServiceMethods.CompareLists(filesRecords.Select(r => r.Name).ToList(), files), "Incorrect files in name column"); 
            Assert.That(filesRecords.Select(r => r.Editor).ToList().TrueForAll(ed => ed.Equals(editor)), "Editor value is not correct");
            foreach (var time in filesRecords.Select(r => r.CreationDate).ToList())
            {
                Assert.That(DateTime.Parse(time), Is.EqualTo(expectedTime).Within(1).Minutes, "Incorrect Time");
            }

            var link = App.Ui.OrdersWorkpiece.GetRandomLinkFromFilesGrid();
            var tryToDownloadResponse =
                App.Api.ApiHelp.DownloadFile(link);
            var expectedFileLenght = FileUtils.GetFilesLenght(projectPath + link.Split('/').Last().Replace("%20", " "));
            Assert.AreEqual(tryToDownloadResponse.ContentLength, expectedFileLenght, "File lenght is not equal");
        }
    }
}
