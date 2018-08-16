
namespace Tests.UI.Pages.Machines
{
    using System.Collections.Generic;

    using Core.WeDriverService.Extensions;

    using global::Tests.Models.Machines.UiModels;
    using global::Tests.UI.Components.Machines;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Html5;

    public class MachinesTaskAllocationPage : PageWithGridTemplate
    {

        private By headerLocator = By.XPath("//h1[contains(@class, 'PageTitle')][.='Machines']");

        public MachinesTaskAllocationPage(IWebDriver driver)
            : base(driver)
        {
            this.WaitForPageLoad();
        }

        private TasksAllocationGrid TasksAllocationGrid => new TasksAllocationGrid(Driver);


        public bool Opened()
        {
            return Driver.Displayed(this.headerLocator);
        }

        public List<TaskAllocationRecord> GetMachineTasksInfo()
        {
            return TasksAllocationGrid.GetRecords();
        }

        public List<string> GetGridColumnNames()
        {
            return TasksAllocationGrid.GetColumnsNames();
        }

        public void ClickOnMachine(string machine)
        {
            TasksAllocationGrid.ClickRecord(machine);
        }
    }
}
