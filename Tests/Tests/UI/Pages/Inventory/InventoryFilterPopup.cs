
namespace Tests.UI.Pages.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Core.WeDriverService;
    using Core.WeDriverService.Extensions;

    using global::Tests.TestsData.Inventory.Enums.FilterSearch;
    using global::Tests.UI.Components.General;
    using global::Tests.UI.Pages.General;

    using OpenQA.Selenium;

    public class InventoryFilterPopup : AnyPage
    {
        public InventoryFilterPopup(IWebDriver driver)
            : base(driver)
        {
        }

        #region locators
        
        private readonly By applyButtonLocator = By.XPath("//button[./span[contains(.,'Apply')]]");

        private readonly By cancelButtonLocator = By.XPath("//button[./span[contains(.,'Cancel')]]");

        private readonly By clearAllButonLocator = By.XPath("//button[./span[contains(.,'Clear all')]]");

        private readonly By closeButtonLocator = By.XPath("//button[./*[contains(@class, 'closeIcon')]]");

        private readonly By toolSizeInputLocator = By.CssSelector("input[name='toolSize']");

        private readonly By toolLengthInputLocator = By.CssSelector("input[name='toolLength']");

        private readonly By filterLabelLocator = By.CssSelector("label[class$='label'], div[class$='label']");

        private readonly Dictionary<FilterSearchData.Filters, By> filtersLocators = new Dictionary<FilterSearchData.Filters, By>
                                                         {
                                                            {
                                                                 FilterSearchData.Filters.UsageMaterial,
                                                                By.XPath("//button[@placeholder='Select usage material']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.ToolMaterial,
                                                                 By.XPath("//button[@placeholder='Select material']")
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
                                                                 By.XPath("//button[@placeholder='Select tool group']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.Type,
                                                                 By.XPath("//button[@placeholder='Select type']")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.Cooling,
                                                                 By.XPath("//label[.//div[.='Cooling functionality']]/input")
                                                             },
                                                             {
                                                                 FilterSearchData.Filters.AvaliabilityInStock,
                                                                 By.XPath("//label[.//div[.='Availability in stock']]/input")
                                                             }
                                                         };

        #endregion

        public bool IsOpened => Driver.Displayed(this.applyButtonLocator);

        #region selects

        private CustomSelector UsageMaterialSelector => new CustomSelector(Driver, this.filtersLocators[FilterSearchData.Filters.UsageMaterial]);

        private CustomSelector ToolMaterialSelector => new CustomSelector(this.Driver, this.filtersLocators[FilterSearchData.Filters.ToolMaterial]);

        private CustomSelector ToolGroupSelector => new CustomSelector(this.Driver, this.filtersLocators[FilterSearchData.Filters.ToolGroup]);

        private CustomSelector ToolTypeSelector => new CustomSelector(this.Driver, this.filtersLocators[FilterSearchData.Filters.Type]);
       
        #endregion


        public void WaitForPageLoad()
        {
                Driver.WaitForElementToBeClickable(this.applyButtonLocator);
                Driver.WaitForElementToBeClickable(this.filtersLocators[FilterSearchData.Filters.AvaliabilityInStock]);
                Driver.WaitForElementToBeClickable(this.filtersLocators[FilterSearchData.Filters.Cooling]);
        }


        public void ClickApplyButton()
        {
            Driver.Click(this.applyButtonLocator);
        }

        public void ClickCancelButton()
        {
            Driver.Find(this.cancelButtonLocator).Click();
        }

        public void ClickClearAllButton()
        {
            Driver.Find(this.clearAllButonLocator).Click();
        }

        public void ClickCloseButton()
        {
            Driver.Find(this.closeButtonLocator).Click();
        }

        public void PopulateUsageMaterial(string material)
        {
            this.UsageMaterialSelector.SelectOption(material);
        }

        public void PopulateToolMaterial(string material)
        {
            this.ToolMaterialSelector.SelectOption(material);
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
            this.ToolGroupSelector.SelectOption(group);
        }

        public void PopulateToolType(string type)
        {
            this.ToolTypeSelector.SelectOption(type);
        }

        public void SetCoolingFunctionality(bool select)
        {
            var element = this.Driver.Find(this.filtersLocators[FilterSearchData.Filters.Cooling]);
            if (element.Selected != select)
            {
                // Todo: Remove second click after field focus changing issue is fixed 
                Driver.Click(this.filtersLocators[FilterSearchData.Filters.Cooling]);
               
                int counter = 0;
                while (this.Driver.Find(this.filtersLocators[FilterSearchData.Filters.Cooling]).Selected != select
                       && counter++ < 10)
                {
                    Driver.Click(this.filtersLocators[FilterSearchData.Filters.Cooling]);
                }
            }
        }

        public void SetAvaliabilityInStock(bool select)
        {
            var element = this.Driver.Find(this.filtersLocators[FilterSearchData.Filters.AvaliabilityInStock]);
            if (element.Selected != select)
            {
                // Todo: Remove second click after field focus changing issue is fixed 
                Driver.Click(this.filtersLocators[FilterSearchData.Filters.AvaliabilityInStock]);

                int counter = 0;
                while (this.Driver.Find(this.filtersLocators[FilterSearchData.Filters.AvaliabilityInStock]).Selected
                       != select && counter++ < 10)
                {
                    Driver.Click(this.filtersLocators[FilterSearchData.Filters.AvaliabilityInStock]);
                }
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
            
            return this.Driver.Finds(filterLabelLocator).Select(e => e.Text).ToList();
        }

        public string GetFilterState(FilterSearchData.Filters filter)
        {
            this.WaitForPageLoad();
            if (this.filtersLocators.ContainsKey(filter))
            {
                var dropdowns = new Dictionary<FilterSearchData.Filters, CustomSelector>
                                    {
                                        {
                                            FilterSearchData
                                                .Filters
                                                .UsageMaterial,
                                            UsageMaterialSelector
                                        },
                                        {
                                            FilterSearchData
                                                .Filters
                                                .ToolMaterial,
                                            ToolMaterialSelector
                                        },
                                        {
                                            FilterSearchData
                                                .Filters.ToolGroup,
                                            ToolGroupSelector
                                        },
                                        {
                                            FilterSearchData
                                                .Filters.Type,
                                            ToolTypeSelector
                                        }
                                    };
                
                if (dropdowns.Keys.Contains(filter))
                {
                    var state = dropdowns[filter].SelectedOption();
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
                        var value = filterElement.GetAttribute("value");
                        var state = value == string.Empty ? filterElement.GetAttribute("placeholder") : value;

                        return state;
                    }
                }
            }
            else
            {
                if (filter != FilterSearchData.Filters.Search)
                {
                    throw new Exception($"'{filter}' is not valid filter");
                }

                return null;
            }
        }
    }
}
