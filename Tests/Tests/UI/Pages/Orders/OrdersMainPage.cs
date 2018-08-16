
namespace Tests.UI.Pages.Orders
{
    using System.Collections.Generic;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ProjectManager.UiModels;
    using global::Tests.UI.Components.Orders;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;

    public class OrdersMainPage : PageWithGridTemplate
    {
        #region locators

        private readonly By createNewOrderButtonLocator = By.CssSelector("div[class$='layout_body'] button");

        #endregion

        public OrdersMainPage(IWebDriver driver)
            : base(driver)
        {
            this.WaitForPageLoad();
        }

        #region custom elements

        private OrdersGrid OrdersGrid => new OrdersGrid(this.Driver);

        #endregion

        public List<string> GetOrdersGridColumnsNames()
        {
            this.WaitForPageLoad();
            return this.OrdersGrid.GetColumnsNames();
        }
        
        public List<OrderGridRecord> GetGridRecords()
        {
            return this.OrdersGrid.GetRecords();
        }

        public IReadOnlyCollection<IWebElement> GetRawGridLineElements()
        {
            return this.OrdersGrid.GetGridLineElements();
        }

        public void ClickCreateNewOrderButton()
        {
            this.Driver.Find(this.createNewOrderButtonLocator).Click();
        }

        public void ClickOrder(string orderId)
        {
            this.OrdersGrid.ClickRecord(orderId);
        }

        public bool Opened()
        {
            return Driver.Displayed(this.createNewOrderButtonLocator);
        }
    }
}
