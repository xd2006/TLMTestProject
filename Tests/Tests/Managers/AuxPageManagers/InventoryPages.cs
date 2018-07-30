
namespace Tests.Managers.AuxPageManagers
{
    using global::Tests.Managers.AuxPageManagers.Templates;
    using global::Tests.UI.Pages.Inventory;

    using OpenQA.Selenium;

    public class InventoryPages : PagesManagerTemplate
    {

        /// <summary>
        /// The home page.
        /// </summary>
        private InventoryMainPage inventoryMainPage;

        private InventoryToolInformationPopup toolInfoPopup;

        public InventoryPages(IWebDriver driver)
            : base(driver)
        {
        }
        
        public InventoryMainPage ToolsMainPage => this.inventoryMainPage ?? (this.inventoryMainPage = new InventoryMainPage(this.Driver));

        public InventoryToolInformationPopup ToolInfoPage => this.toolInfoPopup ?? (this.toolInfoPopup = new InventoryToolInformationPopup(this.Driver));

    }
    
}
