
namespace Tests.UI.Components.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Parser.Html;

    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class TaskRequiredToolsGrid : GridTemplate, IGrid<ToolGridRecord>
    {
        public TaskRequiredToolsGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<ToolGridRecord> GetRecords()
        {
            var parser = new HtmlParser();
            List<ToolGridRecord> tools = new List<ToolGridRecord>();

            var doc = parser.Parse(this.Driver.PageSource);

            var gridRowCssSelector = GridRowCssLocatorText;
            var gridCellCssSelector = "td";

            var rows = doc.QuerySelectorAll(gridRowCssSelector);

            foreach (var row in rows)
            {
                var tool = new ToolGridRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);

                if (cells.Length == 1 && cells.First().TextContent.Equals("No data"))
                {
                    break;
                }

                var cellsTexts = cells.Select(t => t.TextContent).ToList();

                tool.Name = cellsTexts[0];

                if (!string.IsNullOrEmpty(cellsTexts[1]))
                {
                    tool.Size = int.Parse(cellsTexts[1]);
                }

                if (!string.IsNullOrEmpty(cellsTexts[2]))
                {
                    tool.Length = int.Parse(cellsTexts[2]);
                }

                tool.Quantity = int.Parse(cellsTexts[3]);
                tool.QuantityInAssignedMachine = int.Parse(cellsTexts[4]);

                tools.Add(tool);
            }

            return tools;       
        }
    }
}
