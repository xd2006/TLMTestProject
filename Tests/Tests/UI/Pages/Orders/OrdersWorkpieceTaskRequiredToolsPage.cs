
namespace Tests.UI.Pages.Orders
{
    using System.Collections.Generic;

    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.UI.Components.Orders;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;

    public class OrdersWorkpieceTaskRequiredToolsPage : PageWithGridTemplate
    {
        public OrdersWorkpieceTaskRequiredToolsPage(IWebDriver driver)
            : base(driver)
        {
        }

        private TaskRequiredToolsGrid toolsGrid => new TaskRequiredToolsGrid(Driver);

        public List<string> GetTableColumnsNames()
        {
            return toolsGrid.GetColumnsNames();
        }

        public List<ToolGridRecord> GetRequiredToolsRecords()
        {
            return toolsGrid.GetRecords();
        }
    }
}
