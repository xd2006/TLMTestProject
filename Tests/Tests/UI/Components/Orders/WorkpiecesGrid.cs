
namespace Tests.UI.Components.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AngleSharp.Parser.Html;

    using Core.Service;
    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class WorkpiecesGrid : GridTemplate, IGrid<WorkpieceGridRecord>
    {
      
        public WorkpiecesGrid(IWebDriver driver)
            : base(driver)
        {
        }

        public List<WorkpieceGridRecord> GetRecords()
        {
           
            List<WorkpieceGridRecord> entities = new List<WorkpieceGridRecord>();
            
            this.Driver.WaitForElements(this.GridRowLocator);
            string error;
            int i = 0;
            do
            {
                try
                {
                    var parser = new HtmlParser();
                    error = string.Empty;
                    var doc = parser.Parse(this.Driver.PageSource);

                    var gridRowCssSelector = GridRowCssLocatorText;
                    var gridCellCssSelector = "td";

                    var rows = doc.QuerySelectorAll(gridRowCssSelector);

                    foreach (var row in rows)
                    {
                        var workpiece = new WorkpieceGridRecord();
                        var cells = row.QuerySelectorAll(gridCellCssSelector);

                        var cellsTexts = cells.Select(t => t.TextContent).ToList();
                        workpiece.Id = cellsTexts[0];
                        workpiece.Name = cellsTexts[1];
                        workpiece.Quantity = cellsTexts[2];
                        workpiece.DeliveryDate = cellsTexts[3];

                        if (Parameters.Parameters.Browser == "MicrosoftEdge")
                        {
                            workpiece.DeliveryDate = ServiceMethods.ConvertEdgeDateToUtf(workpiece.DeliveryDate);
                        }

                        entities.Add(workpiece);
                    }
                }
                catch (Exception e)
                {
                    error = e.Message;
                    Thread.Sleep(500);
                    entities.Clear();
                }
            }
            while (error != string.Empty && i++ < 10);

            if (i >= 10)
            {
                throw new Exception(error);
            }

            return entities;
        }
        

        public new void ClickRecord(string workpieceId)
        {
            By recordNameLocator = By.CssSelector("table tr > td:nth-of-type(1)");

            Func<IWebDriver, IWebElement> method = webDriver => 
                webDriver.Finds(recordNameLocator).FirstOrDefault(t => t.Text.Equals(workpieceId));

            var element = this.Driver.WaitForSpecificElement(method);
            element.Click();
        }
        
    }
}
