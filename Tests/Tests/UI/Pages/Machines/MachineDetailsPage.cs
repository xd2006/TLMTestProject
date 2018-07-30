
namespace Tests.UI.Pages.Machines
{
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;

    using global::Tests.Models.Machines.UiModels;
    using global::Tests.UI.Components.Machines;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Html5;

    public class MachineDetailsPage : PageWithGridTemplate
    {

        private By currentTaskInfoCellLocator = By.XPath(
            "//div[contains(@class,'title')][.='Current task']/following-sibling::div//table/tbody/tr/td");

        private By backButtonLocator = By.CssSelector("button[class$='history-back_overridenButton']");

        private By pageTitleLocator = By.CssSelector("h1[class$='pageTitle']");

        public MachineDetailsPage(IWebDriver driver)
            : base(driver)
        {
        }

        private UpcomingTasksGrid UpcomingTasksGrid => new UpcomingTasksGrid(Driver);

        public TaskAllocationRecord GetCurrentTaskInfo()
        {
            TaskAllocationRecord currentTask = new TaskAllocationRecord();
            var cells = Driver.Finds(this.currentTaskInfoCellLocator).Select(c => c.Text).ToList();

            currentTask.CurrentTask = cells[0];
            currentTask.Workpiece = cells[1];
            currentTask.ActualStart = cells[2];
            currentTask.EstimatedDuration = cells[3];
            currentTask.EstimatedEnd = cells[4];
            currentTask.DeliveryDate = cells[5];

            return currentTask;
        }

        public List<TaskAllocationRecord> GetUpcomingTasks()
        {
            return UpcomingTasksGrid.GetRecords();
        }

        public void ClickBackLink()
        {
            Driver.Find(this.backButtonLocator).Click();
        }

        public string GetMachineName()
        {
           return Driver.Find(this.pageTitleLocator).Text;
        }
    }
}
