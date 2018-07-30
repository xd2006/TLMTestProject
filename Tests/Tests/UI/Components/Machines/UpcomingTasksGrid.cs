
namespace Tests.UI.Components.Machines
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
    using AngleSharp.Parser.Html;

    using global::Tests.Models.Machines.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class UpcomingTasksGrid : GridTemplate, IGrid<TaskAllocationRecord>
    {
        public UpcomingTasksGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<TaskAllocationRecord> GetRecords()
        {          
            List<TaskAllocationRecord> tasks = new List<TaskAllocationRecord>();

            var rows = this.GetGridRows();

            var gridCellCssSelector = "td";
            bool currentTaskRow = true; // to skip current task row
            foreach (var row in rows)
            {
                if (currentTaskRow)
                {
                    currentTaskRow = false;
                    continue;
                }

                var task = new TaskAllocationRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);

                var cellsTexts = cells.Select(t => t.TextContent).ToList();
                task.UpcomingTask = cellsTexts[0];
                task.Workpiece = cellsTexts[1];
                task.ActualStart = cellsTexts[2];
                task.EstimatedDuration = cellsTexts[3];
                task.EstimatedEnd = cellsTexts[4];
                task.DeliveryDate = cellsTexts[5];               
                tasks.Add(task);
            }
            
            return tasks;
        }     
    }
}
