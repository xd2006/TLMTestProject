namespace Tests.UI.Components.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Parser.Html;

    using Core.WeDriverService;

    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class WorkplanTasksGrid : GridTemplate, IGrid<TaskGridRecord>
    {
        public WorkplanTasksGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<TaskGridRecord> GetRecords()
        {
            var parser = new HtmlParser();
            List<TaskGridRecord> entities = new List<TaskGridRecord>();

            var doc = parser.Parse(this.Driver.PageSource);

            var gridRowCssSelector = GridRowCssLocatorText;
            var gridCellCssSelector = "td";

            var rows = doc.QuerySelectorAll(gridRowCssSelector);

            foreach (var row in rows)
            {
                var task = new TaskGridRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);

                var cellsTexts = cells.Select(t => t.TextContent).ToList();

                if (cellsTexts.Count > 1)
                {
                    task.Name = cellsTexts[0];
                    task.DurationPerWorkpiece = cellsTexts[1];
                    task.DurationInTotal = cellsTexts[2];
                    task.Start = cellsTexts[3];
                    task.End = cellsTexts[4];
                    task.Machine = cellsTexts[5];
                    task.CamFile = cellsTexts[6];
                    entities.Add(task);
                }
            }

            return entities;
        }


        public new void ClickRecord(string workplanName)
        {
            By recordNameLocator = By.CssSelector("table tr[class$=_table_row] > td:nth-of-type(1)");
            this.Driver.Finds(recordNameLocator).Where(t => t.Text.Equals(workplanName)).ToList().First().Click();
        }

        public void AddCamFileForTask(string taskName, string filePath)
        {
            var records = this.GetRecords();
            var neededTask = records.First(r => r.Name.Equals(taskName));
            var rows = this.Driver.Finds(this.GridRowLocator).ToList();
            var row = rows[records.IndexOf(neededTask)];
            row.FindElement(By.CssSelector("input.react-fine-uploader-file-input")).SendKeys(filePath);
        }
    }
}
