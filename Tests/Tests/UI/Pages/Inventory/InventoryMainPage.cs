
namespace Tests.UI.Pages.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.Models.ToolManager.UiModels;
    using global::Tests.TestsData.Inventory.Enums.FilterSearch;
    using global::Tests.UI.Components.Tools;
    using global::Tests.UI.Pages.PagesTemplates;

    using OpenQA.Selenium;

    public class InventoryMainPage : PageWithGridTemplate
    {
        
        #region locators
        
        private readonly By filterButtonLocator = By.XPath("//button[./span[contains(.,'Filter')]]"); 

        private readonly By clearFiltersButtonLocator = By.CssSelector("a[class$='shared_clear']");
     
        private readonly By toolTypeButtonLocator = By.CssSelector("div[class$='tab-bar_root'] > div");

        private readonly By clearSearchButton = By.CssSelector("div[class$='closeContainer']");
      
        private readonly Dictionary<FilterSearchData.Filters, By> filtersLocators = new Dictionary<FilterSearchData.Filters, By>
                                                         {
                                                             {
                                                                 FilterSearchData.Filters.Search,
                                                                 By.CssSelector("input[class$='searchInput']")
                                                             }
                                                         };

        private readonly By _toolScoutBtn = By.CssSelector("*[class$='toolScoutLabel']");
        #endregion
        
        public InventoryMainPage(IWebDriver driver)
            : base(driver)
        {
            this.WaitForPageLoad();
        }


        #region custom elements

        private ToolsGrid ToolsGrid => new ToolsGrid(this.Driver);

        #endregion
       
        public List<ToolGridRecord> GetRecords()
        {
            this.WaitForPageLoad();
            return this.ToolsGrid.GetRecords();
        }

        public IReadOnlyCollection<IWebElement> GetRawGridLineElements()
        {
            return this.ToolsGrid.GetGridLineElements();
        }

        public void ClickRecord(string toolName)
        {
            this.ToolsGrid.ClickRecord(toolName);
        }
        
        public void ClearSearch()
        {
            this.Driver.Find(filtersLocators[FilterSearchData.Filters.Search]).Clear();           
        }

        public void ResetFilters()
        {
            Driver.Click(clearFiltersButtonLocator);
        }

        public void ResetFiltersAndSearch()
        {
            Driver.Click(clearFiltersButtonLocator);
            Driver.Click(this.clearSearchButton);
        }

        public string GetSearchFieldValue()
        {
            return this.Driver.FindElement(filtersLocators[FilterSearchData.Filters.Search]).GetAttribute("value");
        }

        public void PopulateSearchField(string searchTerm, bool populateOnly = false)
        {
            this.Driver.Populate(filtersLocators[FilterSearchData.Filters.Search], searchTerm);

            if (!populateOnly)
            {
                Driver.Find(filtersLocators[FilterSearchData.Filters.Search]).SendKeys(Keys.Enter);
            }
        }

        public void ClickEnterInSearchInput()
        {
            Driver.Find(filtersLocators[FilterSearchData.Filters.Search]).SendKeys(Keys.Enter);
        }

        public void ClickFilterButton()
        {
            Driver.Click(filterButtonLocator);
           this.Driver.WaitForPageReady();
        }

        #region Get info methods
        public string GetSearchFieldLabel()
        {
            return this.Driver.FindElement(filtersLocators[FilterSearchData.Filters.Search]).GetAttribute("placeholder");                        
        }
        
        public string GetActiveToolType()
        {
            return this.Driver.Finds(this.toolTypeButtonLocator).ToList()
                .First(e => e.GetAttribute("class").Contains("active")).Text;
        }

        #endregion

        public List<string> GetGridColumnsNames()
        {
            return this.ToolsGrid.GetColumnsNames();
        }
        
        public void SelectToolType(FilterSearchData.ToolsTypes toolsType)
        {
            var elements = this.GetRawGridLineElements();
            var button = this.Driver.Finds(this.toolTypeButtonLocator)
                .First(t => t.Text.Equals(toolsType.ToString()));
            if (!button.GetAttribute("class").Contains("active"))
            {
                button.Click();
                this.Driver.WaitForElementStaleBool(elements.Last(), 10);
            } 
            this.WaitForPageLoad();
        }

        public void ClickColumnName(string name)
        {
            this.ToolsGrid.ClickColumnName(name);
        }

        public List<string> GetGroupsLabels()
        {
            return this.Driver.Finds(this.toolTypeButtonLocator).Select(e => e.Text).ToList();
        }
      
        public bool Opened()
        {
           return Driver.Displayed(toolTypeButtonLocator);
        }

        public void OpenToolScout()
        {
            Driver.Find(_toolScoutBtn).Click();
        }
    }
}
