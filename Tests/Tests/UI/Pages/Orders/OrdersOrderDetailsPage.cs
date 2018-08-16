
namespace Tests.UI.Pages.Orders
{
    using System.Collections.Generic;

    using Core.Service;
    using Core.WeDriverService;

    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.UI.Components.Orders;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;

    public class OrdersOrderDetailsPage : PageWithGridTemplate
    {

        private readonly string orderDataFieldMask =
            "//div[contains(@class,'order-details_fields')]//span[contains(.,'{0}')]/following-sibling::span";

        private readonly By backLinkLocator = By.CssSelector("button[class$='history-back_overridenButton']");

        private readonly By createNewWorkpieceButtonLocator = By.CssSelector("button[class$='order-details_addButton']");

        public OrdersOrderDetailsPage(IWebDriver driver)
            : base(driver)
        {
        }

        private WorkpiecesGrid WorkpiecesGrid => new WorkpiecesGrid(this.Driver);

        public string GetOrderDetailsContent(string fieldName)
        {
            By locator = By.XPath(string.Format(this.orderDataFieldMask, fieldName));
            var text = this.Driver.Find(locator).Text;

            var content = text.Trim();

            if (Parameters.Parameters.Browser == "MicrosoftEdge")
            {
                content = ServiceMethods.ConvertEdgeDateToUtf(content);
            }

            return content;
        }

        public List<string> GetWorkpiecesTableTitles()
        { 
            return this.WorkpiecesGrid.GetColumnsNames();
        }

        public List<WorkpieceGridRecord> GetWorkpieceGridRecords()
        {
            return this.WorkpiecesGrid.GetRecords();
        }

        public void ClickWorkpieceRecord(string workpieceId)
        {
            this.WorkpiecesGrid.ClickRecord(workpieceId);
            this.WaitForPageLoad();
        }

        public void ClickWorkpieceRecord(int index)
        {
            this.WorkpiecesGrid.ClickRecord(index);
            this.WaitForPageLoad();
        }

        public void ClickBackLink()
        {
            this.Driver.Find(this.backLinkLocator).Click();
        }

        public void ClickCreateNewWorkpieceButton()
        {
            Driver.Click(this.createNewWorkpieceButtonLocator);
        }
    }
}
