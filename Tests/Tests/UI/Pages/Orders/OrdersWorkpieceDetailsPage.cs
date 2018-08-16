
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
    using OpenQA.Selenium.Support.UI;

    public class OrdersWorkpieceDetailsPage : PageWithGridTemplate
    {
        private readonly By addTaskButtonLocator = By.CssSelector("button[class*='task-list_addButton']");

        private readonly By backLinkLocator = By.CssSelector("button[class$='history-back_overridenButton']");

        private readonly By newTaskInputFieldsLabelLocator = By.CssSelector("form label");

        private readonly By workpieceInfoField = By.CssSelector("div[class$='workpiece-details_field']");

        private readonly By _tabLocatorsLocator = By.CssSelector("div[class*='tab-bar_tab']");

        private readonly By _filesLinkLocator = By.CssSelector("td > a");
        public OrdersWorkpieceDetailsPage(IWebDriver driver)
            : base(driver)
        {
            this.WaitForPageLoad();
        }

       private WorkplanTasksGrid TasksGrid => new WorkplanTasksGrid(this.Driver);
        private FilesGrid FilesGrid => new FilesGrid(Driver);

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

            workpiece.ExternalWorkpieceId = workpieceDetails["Workpiece ID"];
            workpiece.Name = workpieceDetails["Name"];
            workpiece.Quantity = int.Parse(workpieceDetails["Ordered q-ty"]);
            workpiece.DeliveryDate = DateTime.ParseExact(workpieceDetails["Delivery date"], "M/d/yyyy", CultureInfo.InvariantCulture);

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
        
        public void ClickCreateNewTask()
        {
            this.Driver.Find(this.addTaskButtonLocator).Click();
        }

        public void AddCamFileForTask(string taskName, string camFilePath)
        {
           TasksGrid.AddCamFileForTask(taskName, camFilePath);
        }

        public void ClickCamFile(string taskName)
        {
            TasksGrid.ClickCamFileLink(taskName);
        }

        public List<string> GetTabsNameList()
        {
            return Driver.Finds(_tabLocatorsLocator).Select(el => el.Text.ToLower()).ToList();
        }

        public void NavigateToTab(WorkpieceDetails.WorkpieceDetailsTabs tabName)
        {
            Driver.Finds(_tabLocatorsLocator)
                .First(el => el.Text.Replace(" ", string.Empty).ToLower()
                .Equals(tabName.ToString().ToLower()))
                .Click();
        }

        public List<string> GetFilesGridColumnsNames()
        {
            return FilesGrid.GetColumnsNames();
        }

        public List<FilesGridRecord> GetFilesGridRecords()
        {
            return FilesGrid.GetRecords();
        }

        public string GetUriInLinkFromFilesGrid(string linkName)
        {
            return Driver.Finds(_filesLinkLocator).First(link => link.Text.Equals(linkName)).GetAttribute("href");
        }
    }
}
