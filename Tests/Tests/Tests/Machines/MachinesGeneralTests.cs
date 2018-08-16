
namespace Tests.Tests.Machines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Bogus;

    using Core.Service;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Tests.Machines.Templates;
    using global::Tests.TestsData.Common.Enums;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    using PetaPoco;

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    [Category("Machines")]   
    public class MachinesGeneralTests : MachinesTestTemplate
    {
        [Test]
        [Category("API")]
        public void CheckMachinesRequest()
        {
            Faker f = new Faker();

            var machine = f.PickRandom(App.GraphApi.ProjectManager.GetMachines());
            var apiMachineTasks = App.GraphApi.ProjectManager.GetMachineTasks(machine.id);
            var dbMachineTasks = App.Db.ProjectManager.GetActiveTasksForMachine(machine.id);

            var apiIds = apiMachineTasks.Select(t => t.Id).ToList();
            var dbIds = dbMachineTasks.Select(t => t.Id).ToList();

            var dif = dbIds.Except(apiIds).ToList();

            Assert.That(
                dif.Count == 0 && apiIds.Count.Equals(dbIds.Count),
                $"There is a difference between tasks from API and DB. MachineId: {machine.id} {Environment.NewLine} "
                + $"Different tasks ids: {ServiceMethods.ListToString(dif)}");

            foreach (var id in apiIds)
            {
                var dbTask = dbMachineTasks.First(t => t.Id.Equals(id));

                var apiTask = apiMachineTasks.First(t => t.Id.Equals(id));

                bool valid = dbTask.Name.Equals(apiTask.Name) && dbTask.StartDate.Equals(apiTask.StartDate)
                                                              && dbTask.EndDate.Equals(apiTask.EndDate)
                                                              && dbTask.DurationPerWorkpiece.Equals(
                                                                  apiTask.DurationPerWorkpiece)
                                                              && dbTask.DurationPerTotal.Equals(
                                                                  apiTask.DurationPerTotal);
                Assert.True(valid, "Not all task records from API are equal to Db ones");
            }
        }
        
        [Test]
        [Category("UI")]
        [Retry(2)] // because of false fails during regular run
        [Property("Reference", "TLM-133")]
        [Property("TestCase", "1246")]
        [Property("TestCase", "1248")]
        [Property("TestCase", "1249")]
        [Property("TestCase", "1250")]
        [Property("TestCase", "1253")]
        [Property("TestCase", "1252")]
        [Property("TestCase", "1251")]
        [Property("TestCase", "1254")]
       public void MachineInformation()
        {
            #region Get test data

            string machineToCheck = "Machine 1"; //ToDo: Check for all machines after implementing task sorting logic

            var machine = App.GraphApi.ProjectManager.GetMachines().First(m => m.name.Equals(machineToCheck));
            var apiMachineTasks = App.GraphApi.ProjectManager.GetMachineTasks(machine.id);

            apiMachineTasks.Sort((x, y) => DateTime.Compare(x.StartDate, y.StartDate));

            //Get current task

            var currentTask = this.GetCurrentTask(apiMachineTasks);

            //Get upcoming tasks
            var upcomingTasks = this.GetUpcomingTasks(apiMachineTasks, currentTask);

            #endregion

            var machineInfo = App.Ui.Machines.GetMachinesInfo(true, 50).First(m => m.Machine.Equals(machineToCheck));

            var workplanId = App.Db.ProjectManager.GetTask(currentTask.Id).WorkplanId;
            var workplan = App.Db.ProjectManager.GetWorkplan(workplanId);
            var workpiece = App.Db.ProjectManager.GetWorkpiece(workplan.WorkpieceId);

            Assert.Multiple(
                () =>
                    {
                        Assert.True(machineInfo.WorkpieceName.Equals(workpiece.Name), $"Workpiece name is incorrect. Expected: {workpiece.Name}. Actual: {machineInfo.WorkpieceName}");
                        Assert.True(machineInfo.CurrentTask.Equals(currentTask.Name), $"Current is incorrect. Expected: {currentTask.Name}. Actual: {machineInfo.CurrentTask}");
                        Assert.True(machineInfo.EstimatedEnd.Equals(currentTask.EndDate.ToString("MM/dd/yyyy") + " 12:00:00 AM"), "Estimated end time is wrong");

                        var estimatedDurationActual =
                            ServiceMethods.ConvertDurationFromTimeFormatToSeconds(machineInfo.EstimatedDuration);

                        Assert.True(
                            estimatedDurationActual.Equals(
                                currentTask.DurationPerTotal),
                            $"Estimated end time is wrong. Expected {currentTask.DurationPerTotal}. Actual {estimatedDurationActual}");
                        Assert.True(
                            upcomingTasks.First().Value.Select(e => e.Name).ToList()
                                .Contains(machineInfo.UpcomingWorkpiece));
                    });
        }


        [Test]
        [Retry(2)] // because of false fails during regular run
        [Category("UI")]
        [Property("Reference", "TLM-133")]
        [Property("TestCase", "1247")]
       
        public void CheckMachinesGridColumnsNames()
        {
            List<string> expectedColumnNames = new List<string>
                                                   {
                                                       "Machine",
                                                       "Workpiece name",
                                                       "Current Task",
                                                       "Duration",
                                                       "Planned End",
                                                       "Upcoming workpiece"
                                                   };

            var gridColumnsNames = App.Ui.Machines.GetGridColumnsNames();

            if (Parameters.Parameters.Browser != "MicrosoftEdge")
            {
                expectedColumnNames = ServiceMethods.StringListToUpper(expectedColumnNames);
            }


            Assert.True(gridColumnsNames.SequenceEqual(expectedColumnNames));
        }

        [Test]
        [Retry(2)] // because of false fails during regular run
        [Category("UI")]
        [Property("Reference", "TLM-196")]
        [Property("TestCase", "1261")]
        [Property("TestCase", "1264")]
        [Property("TestCase", "1265")]
        [Property("TestCase", "1266")]
        [Property("TestCase", "1267")]
        [Property("TestCase", "1268")]
        [Property("TestCase", "1269")]
        [Property("TestCase", "1270")]
        [Property("TestCase", "1271")]
        [Property("TestCase", "1272")]
        [Property("TestCase", "1273")]
        [Property("TestCase", "1274")]
        [Property("TestCase", "1275")]
        [Property("TestCase", "1276")]
        public void CheckMachinesDetailsInformation()
        {
            #region Get test data

            string machineToCheck = "Machine 2";

            var machine = App.GraphApi.ProjectManager.GetMachines().First(m => m.name.Equals(machineToCheck));
            var apiMachineTasks = App.GraphApi.ProjectManager.GetMachineTasks(machine.id);

            apiMachineTasks.Sort((x, y) => DateTime.Compare(x.StartDate, y.StartDate));

            //Get current task

            var currentTask = this.GetCurrentTask(apiMachineTasks);

            //Get upcoming tasks
            var upcomingTasks = this.GetUpcomingTasks(apiMachineTasks, currentTask);

            #endregion
         
            App.Ui.Machines.ClickOnMachine(machineToCheck);

            var workpiece = this.GetWorkpieceByTaskId(currentTask.Id);
            var uiCurrentTask = App.Ui.Machines.GetCurrentTaskInformationFromMachineDetails();


            Assert.Multiple(
                () =>
                    {
                        Assert.True(uiCurrentTask.CurrentTask.Equals(currentTask.Name), $"Task name is incorrect. Expected: {currentTask.Name}. Actual: {uiCurrentTask.CurrentTask}");
                        Assert.True(uiCurrentTask.WorkpieceName.Equals(workpiece.Name), $"Workpiece name is incorrect. Expected: {workpiece.Name}. Actual: {uiCurrentTask.WorkpieceName}");
                        Assert.True(uiCurrentTask.ActualStart.Equals(currentTask.StartDate.ToString("MM/dd/yyyy") + " 12:00:00 AM"), "Actual start time is wrong");
                        Assert.True(uiCurrentTask.EstimatedEnd.Equals(currentTask.EndDate.ToString("MM/dd/yyyy") + " 12:00:00 AM"), "Estimated end time is wrong");

                        var estimatedDurationActual =
                            ServiceMethods.ConvertDurationFromTimeFormatToSeconds(uiCurrentTask.EstimatedDuration);

                        Assert.True(
                            estimatedDurationActual.Equals(
                                currentTask.DurationPerTotal),
                            $"Estimated end time is wrong. Expected {currentTask.DurationPerTotal}. Actual {estimatedDurationActual}");
                        Assert.True(
                            uiCurrentTask.DeliveryDate.Equals(workpiece.DeliveryDate.ToString("MM/dd/yyyy")),
                            "Delivery date is wrong");
                    });
            

            // Check 'Next tasks' section
            var expectedUpcomingTasks = DateTasksDictionaryToList(upcomingTasks);

            var uiUpcomingTasks = App.Ui.Machines.GetUpcomingTasksInformationFromMachineDetails(true, 60);

            Assert.True(expectedUpcomingTasks.Count.Equals(uiUpcomingTasks.Count), $"Incorrect number of upcoming tasks is displayed. Expected {expectedUpcomingTasks.Count} but actual {uiUpcomingTasks.Count}");

            foreach (var task in uiUpcomingTasks)
            {
                var expectedTask = expectedUpcomingTasks.First(t => t.Name.Equals(task.UpcomingWorkpiece) 
                                                                    && (t.StartDate.ToString("MM/dd/yyyy") + " 12:00:00 AM").Equals(task.ActualStart));
                
                workpiece = this.GetWorkpieceByTaskId(expectedTask.Id);

                var estimatedDurationActual =
                    ServiceMethods.ConvertDurationFromTimeFormatToSeconds(task.EstimatedDuration);

                bool valid = task.WorkpieceName.Equals(workpiece.Name)
                             && (expectedTask.StartDate.ToString("MM/dd/yyyy") + " 12:00:00 AM").Equals(
                                 task.ActualStart) && expectedTask.DurationPerTotal.Equals(estimatedDurationActual)
                             && (expectedTask.EndDate.ToString("MM/dd/yyyy") + " 12:00:00 AM").Equals(task.EstimatedEnd)
                             && workpiece.DeliveryDate.ToString("MM/dd/yyyy").Equals(task.DeliveryDate);

                Assert.True(valid, "'Next tasks' are not properly displayed");
            }

            // Check tasks order
            bool validOrder = true;
            for (int i = 1; i < uiUpcomingTasks.Count; i++)
            {
                    validOrder = DateTime.Parse(uiUpcomingTasks[i].ActualStart) >= DateTime.Parse(uiUpcomingTasks[i - 1].ActualStart);
                    if (!validOrder)
                    {
                        break;
                    }               
            }

            Assert.True(validOrder, "'Next tasks' are not displayed in the correct order (by Planned Start)");
        }

        [Test]
        [Category("UI")]
        [Property("Reference", "TLM-196")]
        [Property("TestCase", "1263")]
        public void CheckGettingBackFromMachiesDetails()
        {
            var machines = App.GraphApi.ProjectManager.GetMachines().Select(m => m.name);          
            var machineToCheck = new Faker().PickRandom(machines);

            App.Ui.Machines.ClickOnMachine(machineToCheck);
            var machineName = App.Ui.Machines.GetMachineNameFromMachineDetailsPage();
            Assert.True(machineName.Equals(machineToCheck), "Machine details page wasn't opened");

            App.Ui.Machines.ClickBackFromMachineDetails();
            var machinesPageOpened = App.Ui.Main.IsPageOpened(SidePanelData.Sections.Machines, true);
            Assert.True(machinesPageOpened, "Can't navigate back to Machines main page");
        }      
    }
}
