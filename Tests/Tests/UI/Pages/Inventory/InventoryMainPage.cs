
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
    using OpenQA.Selenium.Support.UI;

    public class InventoryMainPage : PageWithGridTemplate
    {
        
        #region locators

        private readonly By usageMaterialSelectLocator = By.CssSelector("select[name='usageMaterial']");

        private readonly By toolMaterialSelectLocator = By.CssSelector("select[name='toolMaterial']");

        private readonly By toolSizeInputLocator = By.CssSelector("input[name='toolSize']");

        private readonly By toolLengthInputLocator = By.CssSelector("input[name='toolLength']");

        private readonly By toolGroupSelectLocator = By.CssSelector("select[name='group']");

        private readonly By toolSubGroupSelectLocator = By.CssSelector("select[name='subGroup']");

        private readonly By coolingFunctionalityCheckBoxLocator = By.CssSelector("#hasCooling");

        private readonly By avaliabilityInStockCheckBoxLocator = By.CssSelector("#isAvailable");
        
        private readonly By searchButton = By.XPath("//button[.='Search']"); 

//        private readonly By searchInputLocator = By.CssSelector("input[class$='searchInput']");

        private readonly By resetButton = By.XPath("//button[.='Reset']");
     
        private readonly By toolTypeButtonLocator = By.CssSelector("div[class$='tab-bar_root'] > div");
      
        private readonly Dictionary<FilterSearchData.Filters, By> filtersLocators = new Dictionary<FilterSearchData.Filters, By>
                                                         {
                                                             {
                                                                 FilterSearchData.Filters.Search,
                                                                 By.CssSelector("input[class$='searchInput']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.UsageMaterial,
                                                                 By.CssSelector("select[name='usageMaterial']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolMaterial,
                                                                 By.CssSelector("select[name='toolMaterial']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolSize,
                                                                 By.CssSelector("input[name='toolSize']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolLength,
                                                                 By.CssSelector("input[name='toolLength']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolGroup,
                                                                 By.CssSelector("select[name='group']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolSubGroup,
                                                                 By.CssSelector("select[name='subGroup']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.Cooling,
                                                                 By.Id("hasCooling")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.AvaliabilityInStock,
                                                                 By.Id("isAvailable")
                                                             }
                                                         };

        #endregion

        #region selects
        
        private SelectElement UsageMaterialSelector => new SelectElement(this.Driver.Find(this.usageMaterialSelectLocator));

        private SelectElement ToolMaterialSelector => new SelectElement(this.Driver.Find(this.toolMaterialSelectLocator));

        private SelectElement ToolGroupSelector => new SelectElement(this.Driver.Find(this.toolGroupSelectLocator));

        private SelectElement ToolSubGroupSelector => new SelectElement(this.Driver.Find(this.toolSubGroupSelectLocator));

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

        public void ResetSearchAndFilters()
        {         
            this.Driver.Find(this.resetButton).Click();
        }

        public string GetSearchFieldValue()
        {
            return this.Driver.FindElement(filtersLocators[FilterSearchData.Filters.Search]).GetAttribute("value");
        }

        public void PopulateSearchField(string searchTerm)
        {
            this.Driver.Populate(filtersLocators[FilterSearchData.Filters.Search], searchTerm);
        }

        public void ClickSearchButton()
        {
            this.Driver.Find(this.searchButton).Click();
            this.Driver.WaitForPageReady();
        }

        #region Get info methods
        public string GetSearchFieldLabel()
        {
            return this.Driver.FindElement(filtersLocators[FilterSearchData.Filters.Search]).GetAttribute("placeholder");                        
        }

        public string GetFilterState(FilterSearchData.Filters filter)
        {       
            this.WaitForPageLoad();
            if (this.filtersLocators.ContainsKey(filter))
            {
                if (new List<FilterSearchData.Filters>
                        {
                            FilterSearchData.Filters.UsageMaterial,
                            FilterSearchData.Filters.ToolMaterial,
                            FilterSearchData.Filters.ToolGroup,
                            FilterSearchData.Filters.ToolSubGroup
                        }.Contains(filter))
                {
                    SelectElement filterElement = new SelectElement(this.Driver.Find(this.filtersLocators[filter]));
                    var state = filterElement.SelectedOption.Text;
                    return state;
                }
                else
                {
                    if (new List<FilterSearchData.Filters>
                            {
                                FilterSearchData.Filters.Cooling,
                                FilterSearchData.Filters.AvaliabilityInStock
                            }.Contains(filter))
                    {
                        IWebElement filterElement = this.Driver.Find(this.filtersLocators[filter]);
                        var state = filterElement.Selected.ToString();
                        return state;
                    }
                    else
                    {
                        IWebElement filterElement = this.Driver.Find(this.filtersLocators[filter]);
                        var state = filterElement.GetAttribute("value");
                        return state;
                    }
                }
            }
            else
            {
                throw new Exception($"'{filter}' is not valid filter");
            }
        }

        public bool IsFilterEnabled(FilterSearchData.Filters filter)
        {
            this.WaitForPageLoad();
            if (this.filtersLocators.ContainsKey(filter))
            {              
                        IWebElement filterElement = this.Driver.Find(this.filtersLocators[filter]);
                        var state = filterElement.Enabled;
                        return state;                                 
            }
            else
            {
                throw new Exception($"'{filter}' is not valid filter");
            }
        }

        public List<string> GetFilterLabels()
        {
            By filterLabelLocator = By.CssSelector("label[class$='filter_label']");
            return this.Driver.Finds(filterLabelLocator).Select(e => e.Text).ToList();
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
        
        public void PopulateUsageMaterial(string material)
        {
            this.UsageMaterialSelector.SelectByText(material);
        }

        public void PopulateToolMaterial(string material)
        {
            this.ToolMaterialSelector.SelectByText(material);
        }

        public void PopulateToolSize(int size)
        {
           this.Driver.Populate(this.toolSizeInputLocator, size.ToString());
        }

        public void PopulateToolLength(int length)
        {
           this.Driver.Populate(this.toolLengthInputLocator, length.ToString());
        }

        public void PopulateToolGroup(string group)
        {
            this.ToolGroupSelector.SelectByText(group);
        }

        public void PopulateToolSubGroup(string group)
        {
            this.ToolSubGroupSelector.SelectByText(group);
        }

        public void SetCoolingFunctionality(bool select)
        {
            var element = this.Driver.FindElement(this.coolingFunctionalityCheckBoxLocator);
            if (element.Selected != select)
            {
                element.Click();
            }
        }

        public void SetAvaliabilityInStock(bool select)
        {
            var element = this.Driver.FindElement(this.avaliabilityInStockCheckBoxLocator);
            if (element.Selected != select)
            {
                element.Click();
            }
        }

        public void SelectToolType(FilterSearchData.ToolsTypes toolsType)
        {
            var elements = this.GetRawGridLineElements();
            var button = this.Driver.Finds(this.toolTypeButtonLocator)
                .First(t => t.Text.Equals(toolsType.ToString()));
            if (!button.GetAttribute("class").Contains("active"))
            {
                button.Click();
                this.Driver.WaitForElementStaleBool(elements.Last(), 5);
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
    }
}
