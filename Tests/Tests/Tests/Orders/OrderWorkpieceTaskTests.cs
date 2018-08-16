
namespace Tests.Tests.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bogus;

    using Core.Service;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.Tests.Orders.Templates;
    using global::Tests.TestsData.Common.Enums;
    using global::Tests.TestsData.Orders.Enums;

    using NUnit.Framework;
    using NUnit.Framework.Interfaces;

    using OpenQA.Selenium;

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("Task")]
    public class OrderWorkpieceTaskTests : OrdersTestTemplate
    {

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

        public void CheckWorkplanDataFieldsInitialState()
        {
            var orderToWork = new OrderGridRecord();
            Faker faker = new Faker();
            List<OrderGridRecord> orders = new List<OrderGridRecord>();

            orders = this.App.Ui.OrdersMain.GetOrderGridRecords();
            orderToWork = faker.PickRandom(orders);
           
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
                    Assert.True(
                        state.Value.Equals("Select Machine"),
                        $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected 'Select Machine'");
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
                            $"Duration per workpieces is incorrect. Expected '{task.DurationPerTotal}' but actual '{ServiceMethods.ConvertDurationFromTimeFormatToSeconds(addedTask.DurationInTotal)}'");
                        Assert.True(
                            addedTask.Start.Equals(task.StartDate.ToString("MM/dd/yyyy")),
                            $"Start date is incorrect. Expected '{task.StartDate:MM/dd/yyyy}' but actual '{addedTask.Start}'");
                        Assert.True(
                            addedTask.End.Equals(task.EndDate.ToString("MM/dd/yyyy")),
                            $"End date is incorrrect. Expected '{task.EndDate:MM/dd/yyyy}' but actual '{addedTask.End}'");

                        var machineName = this.App.GraphApi.ProjectManager.GetMachine(task.MachineId).name;
                        Assert.True(
                            addedTask.Machine.Equals(machineName),
                            $"Machine name is incorrect. Expected '{machineName}' but actual was '{addedTask.Machine}'");
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
                    Assert.True(
                        state.Value.Equals("Select Machine"),
                        $"Field '{state.Key.ToString()}' is not in valid state. Actual state is '{state.Value}' but expected 'Select Machine'");
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


            var tasksUiAfterNavigation = this.App.Ui.OrdersWorkpiece.GetTasksRecords();

            Assert.True(tasksUi.SequenceEqual(tasksUiAfterNavigation), "Created tasks are not displayed correctly");

            var namesOfTasksToCreate = tasksToAdd.Select(t => t.Name).ToList();

            var namesOfCreatedTasks = tasksUi.Select(t => t.Name).ToList();

            Assert.True(namesOfTasksToCreate.SequenceEqual(namesOfCreatedTasks), "Tasks were not added properly");
        }

        [Test]
        [Category("UI")]
        [Category("NoEdge")]
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
        [Category("NoEdge")]
        [Category("ConsoleErrorExpected")]
        [Property("Reference", "TLM-130")]
        [Property("TestCase", "1243")]
        public void CheckInvalidCamFileUpload()
        {

            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan]);

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

            var tasks = App.Ui.OrdersWorkpiece.GetTasksRecords(true, 30);
            var taskName = tasks.First().Name;
            var fileName = "InValidCamFile.hh";
            string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + fileName;

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

            Assert.True(task.CamFile.ToLower().Equals("add file"), "Add cam file button is not displayed");

            var logs = App.Ui.UiHelp.GetBrowserLogs();

            Assert.True(
                logs.Any(
                    l => l.Message.Contains(
                        $"failed to attach file '{fileName}', reason: {fileName} has an invalid extension.")),
                $"Exception for '{fileName}' wasn't found in console");
        }
    


        [Test]
        [Category("UI")]
        [Category("NoEdge")]
        [Property("Reference", "TLM-129")]
        [Property("Reference", "TLM-222")]
        [Property("TestCase", "1750")]
        [Property("TestCase", "1751")]
        [Property("TestCase", "1752")]
        [Property("TestCase", "1753")]
        [Property("TestCase", "1754")]
        [Property("TestCase", "1755")]
        [Property("TestCase", "1756")]
        [Property("TestCase", "1760")]
       

        public void CheckToolListPage()
        {
             #region expected data

            string machine = "Machine 4";
                List<string> expectedColumnsNames =
                    new List<string>
                        {
                            "Tool Assembly Type",
                            "Size",
                            "Length",
                            "Q-ty In Stock",
                            "Q-ty In Assigned Machine"
                        };
            
                List<ToolGridRecord> expectedToolAssemblies = new List<ToolGridRecord>();
                foreach (var assembly in this.requiredToolAssembliesForCamFile)
                {
                    ToolGridRecord tool = new ToolGridRecord()
                                              {
                                                  Name = (string)assembly[0],
                                                  Size = (int)assembly[1],
                                                  Length = (int)assembly[2],
                                                  Quantity = (int)assembly[3],
                                                  QuantityInAssignedMachine = (int)assembly[4]
                                              };
                    expectedToolAssemblies.Add(tool);
                }

                #endregion

                var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

                App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan], machine);

                var fileName = "valid1.h";
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + fileName;
                var taskName = this.App.Preconditions.AddCamFileToFirstWorkpieceTask(
                    ids[GeneralData.ProjectManagerEnitityTypes.Workpiece],
                    path);
            
               this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(
                    ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

                var task = App.Ui.OrdersWorkpiece.GetTasksRecords().First(t => t.Name.Equals(taskName));
                App.Ui.OrdersWorkpiece.ClickCamFile(task.Name);

                var columns = App.Ui.OrdersTask.GetRequiredToolsTableColumnsNames();
                var tools = App.Ui.OrdersTask.GetRequiredToolsTasks();

                Assert.Multiple(
                    () =>
                        {
                            Assert.True(
                                columns.SequenceEqual(ServiceMethods.StringListToUpper(expectedColumnsNames)),
                                "Required tools grid columns names are not correct");
                            Assert.True(
                                tools.SequenceEqual(
                                    expectedToolAssemblies),
                                "Displayed required tools for task are not correct");
                          });
            }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-222")]
        [Property("TestCase", "2286")]
        [Property("TestCase", "2287")]
        [Property("TestCase", "2288")]
        [Property("TestCase", "2289")]
        [Property("TestCase", "2290")]

        public void CheckToolListPageFromToDoPage()
        {
            #region preconditions

            List<ToolGridRecord> expectedToolAssemblies = new List<ToolGridRecord>();
            foreach (var assembly in this.requiredToolAssembliesForCamFile)
            {
                ToolGridRecord tool = new ToolGridRecord()
                                          {
                                              Name = (string)assembly[0],
                                              Size = (int)assembly[1],
                                              Length = (int)assembly[2],
                                              Quantity = (int)assembly[3],
                                              QuantityInAssignedMachine = (int)assembly[4]
                                          };
                expectedToolAssemblies.Add(tool);
            }


            string loadedFile = "valid1.h";
            string machineName = "Machine 4";
            File file = null;

            var machine = App.GraphApi.ProjectManager.GetMachines().First(m => m.name.Equals(machineName));
            var tasks = App.GraphApi.ProjectManager.GetMachineTasks(machine.id);
            if (tasks != null)
            {
                var tasksIds = tasks.Select(t => t.Id);
                if (tasksIds != null)
                {
                    file = App.Db.ProjectManager.GetFiles()
                        .First(f => f.Name.Equals(loadedFile) && tasksIds.Contains(f.TaskId));
                }
            }

            string taskName = string.Empty;

            if (file == null)
            {
                var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

                App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan], machine.name);

                var fileName = "valid1.h";
                string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + fileName;
                taskName = this.App.Preconditions.AddCamFileToFirstWorkpieceTask(
                    ids[GeneralData.ProjectManagerEnitityTypes.Workpiece],
                    path);
            }
            else
            {
                taskName = tasks.First(t => t.Id.Equals(file.TaskId)).Name;
            }

            #endregion

            App.Ui.Main.NavigateToSectionInSideMenu(SidePanelData.Sections.Todo);
            App.Ui.ToDo.ClickOnPickList(taskName);

            var tools = App.Ui.OrdersTask.GetRequiredToolsTasks();
            
                        Assert.True(
                            tools.SequenceEqual(
                                expectedToolAssemblies),
                            "Displayed required tools for task are not correct");            
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-222")]
        [Property("TestCase", "2292")]
       
        public void CheckCamFileWithInvalidContentUpload()
        {
            var ids = this.App.Preconditions.CreateOrderWorkpieceWorkplan();

            App.Preconditions.CreateTask(ids[GeneralData.ProjectManagerEnitityTypes.Workplan]);

            var fileName = "InvalidDataCamFile.h";
            string path = TestContext.CurrentContext.TestDirectory + "\\TestsData\\Orders\\Files\\" + fileName;
            var taskName = this.App.Preconditions.AddCamFileToFirstWorkpieceTask(
                ids[GeneralData.ProjectManagerEnitityTypes.Workpiece],
                path);

            this.App.Ui.OrdersMain.OpenWorkpieceDetailsDirectly(
                ids[GeneralData.ProjectManagerEnitityTypes.Workpiece]);

            var task = App.Ui.OrdersWorkpiece.GetTasksRecords().First(t => t.Name.Equals(taskName));
            App.Ui.OrdersWorkpiece.ClickCamFile(task.Name);

            var tools = App.Ui.OrdersTask.GetRequiredToolsTasks();

            Assert.True(tools.Count == 0, "Tools shouldn't be displayed since cam file's content is invalid");
        }

        #region test data

        private List<List<object>> requiredToolAssembliesForCamFile = new List<List<object>>
                                                        {
                                                            new List<object>
                                                                {
                                                                    "420X120C030S0120",
                                                                    42000000,
                                                                    152000000,
                                                                    6,
                                                                    4
                                                                },
                                                            new List<object>
                                                                {
                                                                    "060S084C000F2022",
                                                                    6000000,
                                                                    116000000,
                                                                    4,
                                                                    3
                                                                },
                                                            new List<object>
                                                                {
                                                                    "200S123C000F5254",
                                                                    20000000,
                                                                    155000000,
                                                                    10,
                                                                    6
                                                                },
                                                            new List<object>
                                                                {
                                                                    "060S097S020S3535",
                                                                    6000000,
                                                                    129000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "030B048CV0153059",
                                                                    3000000,
                                                                    153000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "042B036SV0142048",
                                                                    4200000,
                                                                    142000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "050B048SV0156062",
                                                                    5000000,
                                                                    156000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "055B048SV0156062",
                                                                    5500000,
                                                                    156000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "085B049SV0160066",
                                                                    8500000,
                                                                    160000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "175B071SV0193098",
                                                                    17500000,
                                                                    193000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "020B036SV0116040",
                                                                    2000000,
                                                                    116000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "100B049SV0160066",
                                                                    10000000,
                                                                    160000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "030B090CH0223110",
                                                                    3000000,
                                                                    223000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "050G015SM0205048",
                                                                    5000000,
                                                                    205000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "060G018SM0211054",
                                                                    6000000,
                                                                    211000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "100G030SM0230073",
                                                                    10000000,
                                                                    230000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "120F095S00127033",
                                                                    12000000,
                                                                    127000000,
                                                                    0,
                                                                    0
                                                                },
                                                            new List<object>
                                                                {
                                                                    "030Z082CJUNKER20",
                                                                    3000000,
                                                                    114000000,
                                                                    0,
                                                                    0
                                                                }
                                                        };

        #endregion

    }
}
