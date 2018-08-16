
namespace Tests.UI.Components.Machines
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

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

        public List<TaskAllocationRecord> GetRecords(bool shouldFind, int timeoutSec)
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
                bool currentTaskRow = true; // to skip current task row
            

            var tasks = new List<TaskAllocationRecord>(); 
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
                task.UpcomingWorkpiece = cellsTexts[0];
                task.WorkpieceName = cellsTexts[1];
                task.ActualStart = cellsTexts[2];
                task.EstimatedDuration = cellsTexts[3];
                task.EstimatedEnd = cellsTexts[4];
                task.DeliveryDate = cellsTexts[5];               
                tasks.Add(task);
            }
            
            return tasks;
        }

        public List<TaskAllocationRecord> GetRecords()
        {
            return this.GetRecords(false, 1);
        }
    }
}
