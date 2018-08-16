namespace Tests.UI.Components.ToDo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AngleSharp.Dom;

    using Core.WeDriverService;

    using global::Tests.Models.ToDo.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class ToDoJobsGrid : GridTemplate, IGrid<ToDoJobRecord>
    {
        private By createPicklistButtonLocator = By.CssSelector("table > tbody > tr > td > button");
        
        public ToDoJobsGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<ToDoJobRecord> GetRecords(bool shouldFind, int timeoutSec)
        {
            int rowsNumber = 0;
            int counter = 0;
            IHtmlCollection<IElement> rows;
            do
            {
                rows = this.GetGridRows();
                rowsNumber = rows.Length;
                if (counter > 0 && rows.Length <= 1)
                {
                    Thread.Sleep(1000);
                }
            }
            while (shouldFind && rowsNumber <= 1 && counter++ < timeoutSec);

            var gridCellCssSelector = "td";
           


            var jobs = new List<ToDoJobRecord>();
            foreach (var row in rows)
            {
               var task = new ToDoJobRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);

                var cellsTexts = cells.Select(t => t.TextContent).ToList();
                task.Task = cellsTexts[0];
                task.Workpiece = cellsTexts[1];
                task.DueDate = cellsTexts[2];
                task.Picklist = cells.Last().QuerySelector("span").TextContent; 
                jobs.Add(task);
            }

            return jobs;
        }

        public List<ToDoJobRecord> GetRecords()
        {
            return this.GetRecords(false, 1);
        }

        public void ClickCreatePicklist(string taskName)
        {
            var records = this.GetRecords(true, 20);
            var index = records.IndexOf(records.First(r => r.Task.Equals(taskName)));
            Driver.FindElements(this.createPicklistButtonLocator).ToList()[index].Click();
        }
    }
}
