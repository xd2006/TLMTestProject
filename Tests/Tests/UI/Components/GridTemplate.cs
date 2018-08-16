
namespace Tests.UI.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
    using AngleSharp.Parser.Html;

    using Core.WeDriverService;

    using OpenQA.Selenium;

    public abstract class GridTemplate : ComponentTemplate
    {
        public GridTemplate(IWebDriver driver)
            : base(driver)
        {
        }

        protected static string GridRowCssLocatorText = "table > tbody > tr";

        protected static By ColumnNameLocator = By.CssSelector("thead > tr > th");
        
        protected readonly By GridRowLocator = By.CssSelector(GridRowCssLocatorText);

        protected readonly By RecordNameLocator = By.CssSelector("table tbody tr > td:nth-of-type(1)");

        public const string GridCellCssSelector = "td";

        public IReadOnlyCollection<IWebElement> GetGridLineElements()
        {
            return this.Driver.Finds(this.GridRowLocator);
        }

        public List<string> GetColumnsNames()
        {
            return this.Driver.Finds(ColumnNameLocator).Select(e => e.Text).ToList().Where(e => e != string.Empty)
                .ToList();
        }

        public void ClickRecord(string recordName)
        {
            By recordNameLocator = By.CssSelector("table tbody tr > td:nth-of-type(1)");
            this.Driver.Finds(recordNameLocator).Where(t => t.Text.Equals(recordName)).ToList().First().Click();
        }

        protected IHtmlCollection<IElement> GetGridRows()
        {
            var parser = new HtmlParser();
            var doc = parser.Parse(this.Driver.PageSource);
            var rows = doc.QuerySelectorAll(GridRowCssLocatorText);
            return rows;
        }
    }
}
