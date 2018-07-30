
namespace Tests.UI.Components.Machines
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Parser.Html;

    using Core.WeDriverService;

    using global::Tests.Models.Machines.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class TasksAllocationGrid : GridTemplate, IGrid<TaskAllocationRecord>
    {
        public TasksAllocationGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<TaskAllocationRecord> GetRecords()
        {
            var parser = new HtmlParser();
            List<TaskAllocationRecord> tasks = new List<TaskAllocationRecord>();

            var doc = parser.Parse(this.Driver.PageSource);

            var gridRowCssSelector = GridRowCssLocatorText;
            var gridCellCssSelector = "td";

            var rows = doc.QuerySelectorAll(gridRowCssSelector);

            foreach (var row in rows)
            {
                var taskAllocation = new TaskAllocationRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);
                
                var cellsTexts = cells.Select(t => t.TextContent).ToList();
                taskAllocation.Machine = cellsTexts[0];
                taskAllocation.Workpiece = cellsTexts[1];
                taskAllocation.Qty = cellsTexts[2];
                taskAllocation.CurrentTask = cellsTexts[3];
                taskAllocation.ActualStart = cellsTexts[4];
                taskAllocation.EstimatedDuration = cellsTexts[5];
                taskAllocation.EstimatedEnd = cellsTexts[6];
                taskAllocation.UpcomingTask = cellsTexts[7];
                tasks.Add(taskAllocation);
            }

            return tasks;
        }
    }
}
