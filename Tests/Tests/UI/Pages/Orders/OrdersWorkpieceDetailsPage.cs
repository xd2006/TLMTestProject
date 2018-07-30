
namespace Tests.UI.Pages.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Core.WeDriverService;

    using global::Tests.Models.ProjectManager.DbModels.Postgres;
    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.TestsData.Orders.Enums;
    using global::Tests.UI.Components.Orders;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class OrdersWorkpieceDetailsPage : PageWithGridTemplate
    {
        private readonly By addTaskButtonLocator = By.CssSelector("button[class*='new-task']");

        private readonly By backLinkLocator = By.CssSelector("button[class$='history-back_overridenButton']");

        private readonly By newTaskInputFieldsLabelLocator = By.CssSelector("label[class$='new-task_label']");

        private readonly By workpieceInfoField = By.CssSelector("div[class$='workpiece-details_field']");

        private readonly By nameInputLocator = By.CssSelector("input[name='name']");

        private readonly By durationPerWorkpieceInputLocator = By.CssSelector("input[name='durationPerWorkpiece']");

        private readonly By durationInTotalInputLocator = By.CssSelector("input[name='durationPerTotal']");

        private readonly By startInputLocator = By.CssSelector("input[name='startDate']");

        private readonly By endInputLocator = By.CssSelector("input[name='endDate']");

        private readonly By machineSelectLocator = By.CssSelector("select[name='machineId']");

        public OrdersWorkpieceDetailsPage(IWebDriver driver)
            : base(driver)
        {
        }

        #region selects

        private SelectElement MachineSelector => new SelectElement(this.Driver.Find(this.machineSelectLocator));

        #endregion

        private Dictionary<WorkpieceDetails.TaskFields, By> FieldsLocators =>
            new Dictionary<WorkpieceDetails.TaskFields, By>
                {
                    { WorkpieceDetails.TaskFields.Name, this.nameInputLocator },
                    { WorkpieceDetails.TaskFields.DurationPerWorkpiece, this.durationPerWorkpieceInputLocator},
                    { WorkpieceDetails.TaskFields.DurationInTotal, this.durationInTotalInputLocator },
                    { WorkpieceDetails.TaskFields.Start, this.startInputLocator },
                    { WorkpieceDetails.TaskFields.End, this.endInputLocator },
                    { WorkpieceDetails.TaskFields.Machine, this.machineSelectLocator },
                };

        private WorkplanTasksGrid TasksGrid => new WorkplanTasksGrid(this.Driver);

        public Workpiece GetWorkpieceData()
        {
            Workpiece workpiece = new Workpiece();
            var workpieceDetailsParentElements = this.Driver.Finds(this.workpieceInfoField);
            Dictionary<string, string> workpieceDetails = new Dictionary<string, string>();
            foreach (var element in workpieceDetailsParentElements)
            {
                var data = element.FindElements(By.CssSelector("span"));
                workpieceDetails.Add(data[0].Text.Trim(), data[1].Text.Trim());
            }

            workpiece.ExternalWorkpieceId = workpieceDetails["ID"];
            workpiece.Name = workpieceDetails["Name"];
            workpiece.Quantity = int.Parse(workpieceDetails["Quantity"]);
            workpiece.DeliveryDate = DateTime.ParseExact(workpieceDetails["Workpiece Delivery Date"], "M/d/yyyy", CultureInfo.InvariantCulture);

            return workpiece;
        }

        public List<string> GetTasksGridColumnsNames()
        {
            return this.TasksGrid.GetColumnsNames();
        }

        public List<TaskGridRecord> GetTasksGridRecords()
        {
            return this.TasksGrid.GetRecords();
        }

        public void ClickBackLink()
        {
           this.Driver.Find(this.backLinkLocator).Click();           
        }

        public List<string> GetNewTaskInputFieldsLabels()
        {
            return this.Driver.Finds(this.newTaskInputFieldsLabelLocator).Select(e => e.Text).ToList();
        }

        public string GetFieldState(WorkpieceDetails.TaskFields field)
        {
            if (field.Equals(WorkpieceDetails.TaskFields.Machine))
            {
                return this.MachineSelector.SelectedOption.Text;
            }

            return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("value");
        }

        public string GetFieldPlaceholder(WorkpieceDetails.TaskFields field)
        {           
                return this.Driver.Find(this.FieldsLocators[field]).GetAttribute("placeholder") ?? string.Empty;                      
        }
       
        public void PopulateField(WorkpieceDetails.TaskFields field, string value)
        {
            if (field.Equals(WorkpieceDetails.TaskFields.Machine))
            {
                this.MachineSelector.SelectByText(value);
            }
            else
            {
                this.Driver.Find(this.FieldsLocators[field]).SendKeys(value);
            }
        }

        public void ClickAddTask()
        {
            this.Driver.Find(this.addTaskButtonLocator).Click();
        }

        public void AddCamFileForTask(string taskName, string camFilePath)
        {
           TasksGrid.AddCamFileForTask(taskName, camFilePath);
        }
    }
}
