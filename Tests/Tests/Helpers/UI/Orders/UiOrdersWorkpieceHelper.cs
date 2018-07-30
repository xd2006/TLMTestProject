
namespace Tests.Helpers.UI.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using global::Tests.Managers;
    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders.Enums;

    public class UiOrdersWorkpieceHelper : UiCommonHelper
    {
        public UiOrdersWorkpieceHelper(ApplicationManager app)
            : base(app)
        {
        }

        public List<string> GetTasksColumnsNames()
        {
            return this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridColumnsNames();
        }

        public void ClickBackLink()
        {
            this.App.Pages.OrdersPages.WorkpieceDetails.ClickBackLink();
            App.Pages.OrdersPages.OrderDetails.WaitForPageLoad();
        }

        public List<string> GetTaskInputFieldsNames()
        {
            return this.App.Pages.OrdersPages.WorkpieceDetails.GetNewTaskInputFieldsLabels();
        }

        public Workpiece GetWorkpieceData()
        {
            return this.App.Pages.OrdersPages.WorkpieceDetails.GetWorkpieceData();
        }

        public Dictionary<WorkpieceDetails.TaskFields, string> GetTaskInputFieldsStates()
        {
            Dictionary<WorkpieceDetails.TaskFields, string> states = new Dictionary<WorkpieceDetails.TaskFields, string>();

            List<WorkpieceDetails.TaskFields> fields =
                new List<WorkpieceDetails.TaskFields>
                    {
                        WorkpieceDetails.TaskFields.Name,
                        WorkpieceDetails.TaskFields.DurationPerWorkpiece,
                        WorkpieceDetails.TaskFields.DurationInTotal,
                        WorkpieceDetails.TaskFields.Start,
                        WorkpieceDetails.TaskFields.End,
                        WorkpieceDetails.TaskFields.Machine
                    };
          
            foreach (var field in fields)
            {
                var state = this.App.Pages.OrdersPages.WorkpieceDetails.GetFieldState(field);
                states.Add(field, state);
            }
            
            return states;
        }

        public Dictionary<WorkpieceDetails.TaskFields, string> GetNewTaskFieldsPlaceholders()
        {
            List<WorkpieceDetails.TaskFields> fields =
                new List<WorkpieceDetails.TaskFields>
                    {
                        WorkpieceDetails.TaskFields.Start,
                        WorkpieceDetails.TaskFields.End,
                     };

            Dictionary<WorkpieceDetails.TaskFields, string> placeholders = new Dictionary<WorkpieceDetails.TaskFields, string>();

            foreach (var field in fields)
            {
                var fieldPlaceholder = this.App.Pages.OrdersPages.WorkpieceDetails.GetFieldPlaceholder(field);

                placeholders.Add(field, fieldPlaceholder);
            }
            
            return placeholders;
        }

        public void AddNewTask(Task task, bool populateOnly = false)
        {
            var machine = this.App.GraphApi.ProjectManager.GetMachine(task.MachineId);
            
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.Name, task.Name);
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.DurationPerWorkpiece,
                task.DurationPerWorkpiece.ToString());
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.DurationInTotal, task.DurationPerTotal.ToString());
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.Start, task.StartDate.ToString("yyyy-MM-dd"));
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.End, task.EndDate.ToString("yyyy-MM-dd"));
            this.App.Pages.OrdersPages.WorkpieceDetails.PopulateField(
                WorkpieceDetails.TaskFields.Machine, machine.name);

            if (!populateOnly)
            {
                var initialNumberOfRecords = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords().Count;
                this.App.Pages.OrdersPages.WorkpieceDetails.ClickAddTask();

                int counter = 0; 
                int recordsNumber;
                do
                {
                    var records = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords();
                    recordsNumber = records.Count;

                    if (recordsNumber <= initialNumberOfRecords)
                    {
                        Thread.Sleep(1000);
                    }
                }
                while (recordsNumber <= initialNumberOfRecords && counter++ < 10);

                if (counter >= 10)
                {
                    throw new Exception("Newly added task is not displayed on grid");
                }
            }
        }

        public List<TaskGridRecord> GetTasksRecords()
        {
            return this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords();
        }

        public void AddCamFileForTask(string taskName, string filePath, bool checkAdded = true)
        {
            string defaultAddCamFileButtonText = string.Empty;
            if (checkAdded)
            {
                var taskGridRecord = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords()
                    .First(t => t.Name.Equals(taskName));
                defaultAddCamFileButtonText = taskGridRecord.CamFile.ToLower();
            }


            App.Pages.OrdersPages.WorkpieceDetails.AddCamFileForTask(taskName, filePath);            
            if (checkAdded)
            {
                string camFiletext = string.Empty;
                int counter = 0;

                do
                {
                    var taskGridRecord = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords()
                        .First(t => t.Name.Equals(taskName));
                    camFiletext = taskGridRecord.CamFile;

                    if (camFiletext.ToLower().Equals(defaultAddCamFileButtonText))
                    {
                        Thread.Sleep(1000);
                    }
                }
                while (camFiletext.ToLower() == defaultAddCamFileButtonText && counter++ < 10);

                if (counter >= 10)
                {
                    throw new Exception("Can't add cam file");
                }
            }          
        }     
    }
}
