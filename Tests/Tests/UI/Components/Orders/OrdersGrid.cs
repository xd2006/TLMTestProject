
namespace Tests.UI.Components.Orders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AngleSharp.Css.Values;
    using AngleSharp.Parser.Html;

    using Core.Service;
    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.UI.Components.Interfaces;

    using OpenQA.Selenium;

    public class OrdersGrid : GridTemplate, IGrid<OrderGridRecord>
    {
        public OrdersGrid(IWebDriver driver)
            : base(driver)
        {
        }
    
        public List<OrderGridRecord> GetRecords()
        {

            var parser = new HtmlParser();
            List<OrderGridRecord> entities = new List<OrderGridRecord>();

            var doc = parser.Parse(this.Driver.PageSource);

            var gridRowCssSelector = GridRowCssLocatorText;
            var gridCellCssSelector = "td";

            var rows = doc.QuerySelectorAll(gridRowCssSelector);

            foreach (var row in rows)
            {
                var order = new OrderGridRecord();
                var cells = row.QuerySelectorAll(gridCellCssSelector);
                
                var cellsTexts = cells.Select(t => t.TextContent).ToList();
                order.OrderId = cellsTexts[0];
                order.Customer = cellsTexts[1];
                order.DeliveryDate = cellsTexts[2].Replace("?", string.Empty);

                if (Parameters.Parameters.Browser == "MicrosoftEdge")
                {
                    order.DeliveryDate = ServiceMethods.ConvertEdgeDateToUtf(order.DeliveryDate);
                }


                order.Editor = cellsTexts[3];
                order.CreationDate = cellsTexts[4].Replace("?", string.Empty);

                if (Parameters.Parameters.Browser == "MicrosoftEdge")
                {
                    order.CreationDate = ServiceMethods.ConvertEdgeDateToUtf(order.CreationDate);
                }

                entities.Add(order);
            }

            return entities;
        }

        public new void ClickRecord(string orderId)
        {
            By recordNameLocator = By.CssSelector("table tr > td:nth-of-type(1)");

            Driver.Click(
                () => this.Driver.Finds(recordNameLocator).Where(t => t.Text.Equals(orderId)).ToList().First());
        }
    }
}
