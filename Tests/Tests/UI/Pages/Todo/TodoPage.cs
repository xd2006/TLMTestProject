
namespace Tests.UI.Pages.Todo
{
    using System;
    using System.Collections.Generic;

    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ToDo.UiModels;
    using global::Tests.UI.Components.ToDo;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Html5;

    public class TodoPage : PageWithGridTemplate
    {

        private By headerLocator = By.XPath("//div[contains(@class, 'layout_body')]/div[.='Upcoming Jobs to be done']");

        public TodoPage(IWebDriver driver)
            : base(driver)
        {
        }

        private ToDoJobsGrid JobsGrid => new ToDoJobsGrid(Driver);

        public bool Opened()
        {
            return Driver.Displayed(this.headerLocator);
        }

        public List<ToDoJobRecord> GetJobsRecords(bool shouldFind, int waitToimeSec)
        {
            return JobsGrid.GetRecords(shouldFind, waitToimeSec);
        }

        public void ClickCreatePickList(string taskName)
        {
            JobsGrid.ClickCreatePicklist(taskName);
        }
    }
}
