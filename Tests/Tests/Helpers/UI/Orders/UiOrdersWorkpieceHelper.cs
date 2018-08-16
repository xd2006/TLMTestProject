
using Core.Utils;

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

            if (!App.Pages.OrdersPages.CreateTaskPopup.IsOpened())
            {
                App.Pages.OrdersPages.WorkpieceDetails.ClickCreateNewTask();
            }

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
                var state = this.App.Pages.OrdersPages.CreateTaskPopup.GetFieldState(field);
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
                var fieldPlaceholder = this.App.Pages.OrdersPages.CreateTaskPopup.GetFieldPlaceholder(field);

                placeholders.Add(field, fieldPlaceholder);
            }
            
            return placeholders;
        }

        public void AddNewTask(Task task, bool populateOnly = false)
        {
            var machine = this.App.GraphApi.ProjectManager.GetMachine(task.MachineId);

            if (!App.Pages.OrdersPages.CreateTaskPopup.IsOpened())
            {
                App.Pages.OrdersPages.WorkpieceDetails.ClickCreateNewTask();
            }

            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.Name, task.Name);
            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.DurationPerWorkpiece,
                task.DurationPerWorkpiece.ToString());
            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.DurationInTotal, task.DurationPerTotal.ToString());
            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.Start, task.StartDate.ToString("yyyy-MM-dd"));
            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.End, task.EndDate.ToString("yyyy-MM-dd"));
            this.App.Pages.OrdersPages.CreateTaskPopup.PopulateField(
                WorkpieceDetails.TaskFields.Machine, machine.name);

            if (!populateOnly)
            {
                var initialNumberOfRecords = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords().Count;
              App.Pages.OrdersPages.CreateTaskPopup.ClickAddTaskButton();
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
                while (recordsNumber <= initialNumberOfRecords && counter++ < 15);

                if (counter >= 10)
                {
                    throw new Exception("Newly added task is not displayed on grid");
                }
            }
        }

        public List<TaskGridRecord> GetTasksRecords(bool shouldFound = true, int timeOutSec = 20)
        {
            List<TaskGridRecord> records = new List<TaskGridRecord>();
            int count;
            int counter = 0;
            do
            {
                records = this.App.Pages.OrdersPages.WorkpieceDetails.GetTasksGridRecords();
                count = records.Count;
                if (count == 0 && counter > 0)
                {
                    Thread.Sleep(1000);
                }
            }
            while (shouldFound && count == 0 && counter++ < timeOutSec);

            return records;

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

        public void ClickCamFile(string task)
        {
            App.Pages.OrdersPages.WorkpieceDetails.ClickCamFile(task);
        }

        public bool IsSaveWorkpieceButtonEnabled()
        {
            return this.App.Pages.OrdersPages.CreateWorkpiecePopup.IsSaveButtonEnabled();
        }

        public void ClickSaveButton()
        {
            App.Pages.OrdersPages.CreateWorkpiecePopup.ClickSaveButton();
        }

        public List<string> GetWorkpieceFieldsNames()
        {
            return App.Pages.OrdersPages.CreateWorkpiecePopup.GetFieldsNames();
        }

        public Dictionary<CreateWorkpiecePopup.WorkpieceFields, string> GetCreateWorkpieceFieldsPlaceholders()
        {
            List<CreateWorkpiecePopup.WorkpieceFields> fields =
                new List<CreateWorkpiecePopup.WorkpieceFields>
                    {
                        CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate
                    };

            Dictionary<CreateWorkpiecePopup.WorkpieceFields, string> placeholders =
                new Dictionary<CreateWorkpiecePopup.WorkpieceFields, string>();

            foreach (var field in fields)
            {
                var fieldPlaceholder = this.App.Pages.OrdersPages.CreateWorkpiecePopup.GetFieldPlaceholder(field);
                placeholders.Add(field, fieldPlaceholder);
            }

            return placeholders;
        }

        public Dictionary<CreateWorkpiecePopup.WorkpieceFields, string> GetWorkpieceInputFieldsStates()
        {
            Dictionary<CreateWorkpiecePopup.WorkpieceFields, string> states = new Dictionary<CreateWorkpiecePopup.WorkpieceFields, string>();

            List<CreateWorkpiecePopup.WorkpieceFields> fields =
                new List<CreateWorkpiecePopup.WorkpieceFields>
                    {
                        CreateWorkpiecePopup.WorkpieceFields.WorkpieceId,
                        CreateWorkpiecePopup.WorkpieceFields.WorkpieceName,
                        CreateWorkpiecePopup.WorkpieceFields.WorkpieceQuantity,
                        CreateWorkpiecePopup.WorkpieceFields.WorkpieceDeliveryDate                     
                    };

            foreach (var field in fields)
            {
                var state = this.App.Pages.OrdersPages.CreateWorkpiecePopup.GetFieldState(field);
                states.Add(field, state);
            }

            return states;
        }

        public void PopulateField(CreateWorkpiecePopup.WorkpieceFields field, string value)
        {        
           this.App.Pages.OrdersPages.CreateWorkpiecePopup.PopulateField(field, value);           
        }

        public void ClickCreateNewTaskButton()
        {
            App.Pages.OrdersPages.WorkpieceDetails.ClickCreateNewTask();
        }

        public void AddFilesForWorkpiece(List<string> files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    App.Pages.OrdersPages.CreateWorkpiecePopup.AddFileForWorkpice(file);
                }
            }
        }

        public void DeleteFilesForWorkpiece(List<string> files)
        {
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    App.Pages.OrdersPages.CreateWorkpiecePopup.DeleteFileForWorkpice(file);
                }
            }
        }

        public List<string> GetTabListForWorkpieceDetailes()
        {
            return App.Pages.OrdersPages.WorkpieceDetails.GetTabsNameList();
        }

        public void NavigateToTab(WorkpieceDetails.WorkpieceDetailsTabs tabName)
        {
            App.Pages.OrdersPages.WorkpieceDetails.NavigateToTab(tabName);
        }

        public List<string> GetFilesGridColumnNames 
            => App.Pages.OrdersPages.WorkpieceDetails.GetFilesGridColumnsNames();

        public List<FilesGridRecord> GetFilesGridRecords
            => App.Pages.OrdersPages.WorkpieceDetails.GetFilesGridRecords();

        public string GetRandomLinkFromFilesGrid()
        {
            var grid = GetFilesGridRecords;
            var index = new Random().Next(0, grid.Count);
            return App.Pages.OrdersPages.WorkpieceDetails.GetUriInLinkFromFilesGrid(grid[index].Name);
        }
    }
}
